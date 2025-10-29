using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectBilling.EnsureSet.Test;

partial class ProjectBillingSetEnsureHandlerTest
{
    [Fact]
    public static async Task HandleAsync_InputIsNull_ExpectFailure()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbTimesheetSet);
        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));

        var handler = new ProjectBillingSetEnsureHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var actual = await handler.HandleAsync(null, TestContext.Current.CancellationToken);
        var expected = Failure.Create(HandlerFailureCode.Persistent, "Input must be not null.");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_InputIsNotNull_ExpectDbTimesheetSetQueryCalledOnce()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbTimesheetSet);

        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));
        var handler = new ProjectBillingSetEnsureHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var input = new ProjectBillingSetEnsureIn(
            callerUserId: new("4c184b5a-dcef-4dca-a7f0-c628c26509f9"),
            billingPeriodId: new("65f588f2-d40c-49b9-9d8a-229614cc1a12"));

        _ = await handler.HandleAsync(input, TestContext.Current.CancellationToken);

        var expectedQuery = new DbSelectQuery("gg_timesheetactivity", "t")
        {
            SelectedFields = new("t.gg_finproject_id AS ProjectId"),
            Filter = new DbCombinedFilter(DbLogicalOperator.And)
            {
                Filters =
                [
                    new DbRawFilter(
                        "t.gg_finproject_id IS NOT NULL"),
                    new DbExistsFilter(
                        selectQuery: new(
                            tableName: "gg_employee_cost_period",
                            tableAlias: "p")
                        {
                            Top = 1,
                            SelectedFields = new("1"),
                            Filter = new DbCombinedFilter(DbLogicalOperator.And)
                            {
                                Filters =
                                [
                                    new DbParameterFilter(
                                        "p.gg_employee_cost_periodid",
                                        DbFilterOperator.Equal,
                                        Guid.Parse("65f588f2-d40c-49b9-9d8a-229614cc1a12"),
                                        "periodId"),
                                    new DbRawFilter("p.gg_from_date <= t.gg_date AND p.gg_to_date >= t.gg_date")
                                ]
                            }
                        }),
                    new DbNotExistsFilter(
                        selectQuery: new(
                            tableName: "gg_project_billing_period",
                            tableAlias: "bp")
                        {
                            Top = 1,
                            SelectedFields = new("1"),
                            Filter = new DbCombinedFilter(DbLogicalOperator.And)
                            {
                                Filters =
                                [
                                    new DbRawFilter("bp.gg_project_id = t.gg_finproject_id"),
                                    new DbParameterFilter(
                                        "bp.gg_billingperiod_id",
                                        DbFilterOperator.Equal,
                                        Guid.Parse("65f588f2-d40c-49b9-9d8a-229614cc1a12"),
                                        "periodId")
                                ]
                            }
                        }),
                ]
            },
            GroupByFields = new("t.gg_finproject_id")
        };

        mockSqlApi.Verify(f => f.QueryEntitySetOrFailureAsync<DbTimesheet>(expectedQuery, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public static async Task HandleAsync_DbTimesheetSetQueryIsFailure_ExpectTransientFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some failure text.");

        var mockSqlApi = BuildMockSqlApi(dbFailure);

        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));
        var handler = new ProjectBillingSetEnsureHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, TestContext.Current.CancellationToken);
        var expected = Failure.Create(HandlerFailureCode.Transient, "Some failure text.", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(
        nameof(ProjectBillingSetEnsureHandlerSource.InputCreateTestData),
        MemberType = typeof(ProjectBillingSetEnsureHandlerSource))]
    internal static async Task HandleAsync_DbResultsAreSuccesses_ExpectDataverseCreateCalledOnce(
        ProjectBillingSetEnsureIn input,
        FlatArray<DbTimesheet> dbTimesheets,
        FlatArray<DataverseEntityCreateIn<ProjectBillingPeriodJson>> expectedInputs)
    {
        var mockSqlApi = BuildMockSqlApi(dbTimesheets);
        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));

        var handler = new ProjectBillingSetEnsureHandler(mockSqlApi.Object, mockDataverseApi.Object);

        _ = await handler.HandleAsync(input, TestContext.Current.CancellationToken);

        foreach (var expectedInput in expectedInputs)
        {
            mockDataverseApi.Verify(f => f.CreateEntityAsync(expectedInput, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, HandlerFailureCode.Transient)]
    [InlineData(DataverseFailureCode.Unauthorized, HandlerFailureCode.Transient)]
    [InlineData(DataverseFailureCode.RecordNotFound, HandlerFailureCode.Transient)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, HandlerFailureCode.Transient)]
    [InlineData(DataverseFailureCode.UserNotEnabled, HandlerFailureCode.Persistent)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, HandlerFailureCode.Persistent)]
    [InlineData(DataverseFailureCode.Throttling, HandlerFailureCode.Transient)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, HandlerFailureCode.Transient)]
    [InlineData(DataverseFailureCode.InvalidPayload, HandlerFailureCode.Transient)]
    [InlineData(DataverseFailureCode.InvalidFileSize, HandlerFailureCode.Transient)]
    public static async Task HandleAsync_DataverseCreateResultIsNotDuplicateRecordFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, HandlerFailureCode expectedFailureCode)
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbTimesheetSet);

        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure message");

        var mockDataverseApi = BuildMockDataverseApi(dataverseFailure);
        var handler = new ProjectBillingSetEnsureHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, TestContext.Current.CancellationToken);
        var expected = Failure.Create(expectedFailureCode, "Some failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_DataverseCreateResultIsDuplicateRecordFailure_ExpectSuccess()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbTimesheetSet);

        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(DataverseFailureCode.DuplicateRecord, "Some failure message");

        var mockDataverseApi = BuildMockDataverseApi(dataverseFailure);
        var handler = new ProjectBillingSetEnsureHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, TestContext.Current.CancellationToken);
        var expected = Result.Success<Unit>(default);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_DataverseCreateResultIsSuccess_ExpectSuccess()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbTimesheetSet);
        var mockDataverseApi = BuildMockDataverseApi(Result.Success<Unit>(default));

        var handler = new ProjectBillingSetEnsureHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, TestContext.Current.CancellationToken);
        var expected = Result.Success<Unit>(default);

        Assert.Equal(expected, actual);
    }
}