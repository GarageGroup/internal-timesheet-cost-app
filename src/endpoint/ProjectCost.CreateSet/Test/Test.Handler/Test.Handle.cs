using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.CreateSet.Test;

partial class ProjectCostCreateHandlerTest
{
    [Fact]
    public static async Task HandleAsync_InputIsNull_ExpectFailure()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbTimesheetSet);
        var mockDataverseCreateApi = BuildMockDataverseCreateApi(Result.Success<Unit>(default));
        var mockDataverseApi = BuildMockDataverseApi(mockDataverseCreateApi.Object);

        var handler = new ProjectCostSetCreateHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var actual = await handler.HandleAsync(null, default);
        var expected = Failure.Create(HandlerFailureCode.Persistent, "Input must be not null");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_InputIsNotNull_ExpectSqlApiCalledOnce()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbTimesheetSet);
        var mockDataverseCreateApi = BuildMockDataverseCreateApi(Result.Success<Unit>(default));
        var mockDataverseApi = BuildMockDataverseApi(mockDataverseCreateApi.Object);

        var handler = new ProjectCostSetCreateHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(canceled: false);

        var input = new ProjectCostSetCreateIn(
            costPeriodId: new("a03eb221-654e-4e80-8054-c489d04ef3e2"),
            systemUserId: new("fd7c47d1-bc37-418d-b2fd-9546ce03aa9a"),
            callerUserId: new("5b25be13-5120-4807-979a-c4f879d547b3"),
            employeeCost: 2121);

        _ = await handler.HandleAsync(input, cancellationToken);

        var expectedQuery = new DbSelectQuery("gg_timesheetactivity", "t")
        {
            SelectedFields = new(
                "t.gg_finproject_id AS ProjectId",
                "SUM(t.gg_duration) AS Duration"),
            Filter = new DbCombinedFilter(DbLogicalOperator.And)
            {
                Filters =
                [
                    new DbParameterFilter(
                        "t.ownerid",
                        DbFilterOperator.Equal,
                        Guid.Parse("fd7c47d1-bc37-418d-b2fd-9546ce03aa9a"),
                        "ownerId"),
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
                                        Guid.Parse("a03eb221-654e-4e80-8054-c489d04ef3e2"),
                                        "periodId"),
                                    new DbRawFilter("p.gg_from_date <= t.gg_date AND p.gg_to_date >= t.gg_date")
                                ]
                            }
                        })
                ]
            },
            GroupByFields = new("t.gg_finproject_id")
        };

        mockSqlApi.Verify(f => f.QueryEntitySetOrFailureAsync<DbTimesheet>(expectedQuery, cancellationToken), Times.Once);
    }

    [Fact]
    public static async Task HandleAsync_DbResultIsFailure_ExpectFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some failure text");

        var mockSqlApi = BuildMockSqlApi(dbFailure);
        var mockDataverseCreateApi = BuildMockDataverseCreateApi(Result.Success<Unit>(default));
        var mockDataverseApi = BuildMockDataverseApi(mockDataverseCreateApi.Object);

        var handler = new ProjectCostSetCreateHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Failure.Create(HandlerFailureCode.Transient, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ProjectCostCreateHandlerSource.InputCreateTestData), MemberType = typeof(ProjectCostCreateHandlerSource))]
    internal static async Task HandleAsync_DbResultIsSuccess_ExpectDataverseCreateCalledOnce(
        ProjectCostSetCreateIn input, FlatArray<DbTimesheet> dbTimesheets, FlatArray<DataverseEntityCreateIn<EmployeeProjectCostJson>> expectedInputs)
    {
        var mockSqlApi = BuildMockSqlApi(dbTimesheets);
        var mockDataverseCreateApi = BuildMockDataverseCreateApi(Result.Success<Unit>(default));
        var mockDataverseApi = BuildMockDataverseApi(mockDataverseCreateApi.Object);

        var handler = new ProjectCostSetCreateHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await handler.HandleAsync(input, cancellationToken);

        foreach (var expectedInput in expectedInputs)
        {
            mockDataverseCreateApi.Verify(f => f.CreateEntityAsync(expectedInput, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Theory]
    [MemberData(nameof(ProjectCostCreateHandlerSource.InputImpersonateCreateTestData), MemberType = typeof(ProjectCostCreateHandlerSource))]
    internal static async Task HandleAsync_DbResultIsSuccess_ExpectDataverseImpersonateCalledExactTimes(
        ProjectCostSetCreateIn input, FlatArray<DbTimesheet> dbOutput, Guid expectedCallerId, int expectedImpersonateCount)
    {
        var mockSqlApi = BuildMockSqlApi(dbOutput);
        var mockDataverseCreateApi = BuildMockDataverseCreateApi(Result.Success<Unit>(default));
        var mockDataverseApi = BuildMockDataverseApi(mockDataverseCreateApi.Object);

        var handler = new ProjectCostSetCreateHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await handler.HandleAsync(input, cancellationToken);

        mockDataverseApi.Verify(a => a.Impersonate(expectedCallerId), Times.Exactly(expectedImpersonateCount));
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
    [InlineData(DataverseFailureCode.DuplicateRecord, HandlerFailureCode.Transient)]
    [InlineData(DataverseFailureCode.InvalidPayload, HandlerFailureCode.Transient)]
    [InlineData(DataverseFailureCode.InvalidFileSize, HandlerFailureCode.Transient)]
    public static async Task HandleAsync_DataverseCreateResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, HandlerFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockSqlApi = BuildMockSqlApi(SomeDbTimesheetSet);
        var mockDataverseCreateApi = BuildMockDataverseCreateApi(dataverseFailure);
        var mockDataverseApi = BuildMockDataverseApi(mockDataverseCreateApi.Object);

        var handler = new ProjectCostSetCreateHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_DataverseCreateResultIsSuccess_ExpectSuccess()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbTimesheetSet);
        var mockDataverseCreateApi = BuildMockDataverseCreateApi(Result.Success<Unit>(default));
        var mockDataverseApi = BuildMockDataverseApi(mockDataverseCreateApi.Object);

        var handler = new ProjectCostSetCreateHandler(mockSqlApi.Object, mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.Equal(expected, actual);
    }
}
