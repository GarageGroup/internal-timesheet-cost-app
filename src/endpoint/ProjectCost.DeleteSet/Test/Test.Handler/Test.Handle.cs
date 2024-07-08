using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.DeleteSet.Test;

partial class ProjectCostDeleteHandlerTest
{
    [Theory]
    [MemberData(nameof(ProjectCostDeleteHandlerSource.InputImpersonateDeleteTestData), MemberType = typeof(ProjectCostDeleteHandlerSource))]
    internal static async Task HandleAsync_ExpectDataverseImpersonateCalledExactTimes(
        ProjectCostSetDeleteIn input,
        DataverseEntitySetGetOut<EmployeeProjectCostJson> dataverseSetGetOutput,
        Guid expectedCallerId,
        int expectedImpersonateCount)
    {
        var mockDataverseApi = BuildMockDataverseApi<EmployeeProjectCostJson>(dataverseSetGetOutput, Result.Success<Unit>(default));
        var handler = new ProjectCostSetDeleteHandler(mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await handler.HandleAsync(input, cancellationToken);

        mockDataverseApi.Verify(f => f.Impersonate(expectedCallerId), Times.Exactly(expectedImpersonateCount));
    }

    [Fact]
    public static async Task HandleAsync_ExpectDataverseGetSetCalledOnce()
    {
        var mockDataverseApi = BuildMockDataverseApi<EmployeeProjectCostJson>(SomeEmployeeProjectCostJsonOut, Result.Success<Unit>(default));

        var handler = new ProjectCostSetDeleteHandler(mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        var input = new ProjectCostSetDeleteIn(
            callerUserId: new("9cdd9452-6872-4798-ad3b-6b9819d9d577"),
            costPeriodId: new("80738293-e49b-4c3f-966d-52afc9964da2"),
            maxItems: 10);

        _ = await handler.HandleAsync(input, cancellationToken);

        var expectedInput = new DataverseEntitySetGetIn(
            entityPluralName: "gg_employee_project_costs",
            selectFields: ["gg_employee_project_costid"],
            filter: "_gg_period_id_value eq '80738293-e49b-4c3f-966d-52afc9964da2' and createdonbehalfby ne null")
        {
            MaxPageSize = 10
        };

        mockDataverseApi.Verify(f => f.GetEntitySetAsync<EmployeeProjectCostJson>(expectedInput, cancellationToken), Times.Once);
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
    public static async Task HandleAsync_DataverseGetSetResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, HandlerFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockDataverseApi = BuildMockDataverseApi<EmployeeProjectCostJson>(dataverseFailure, Result.Success<Unit>(default));
        var handler = new ProjectCostSetDeleteHandler(mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ProjectCostDeleteHandlerSource.InputDeleteTestData), MemberType = typeof(ProjectCostDeleteHandlerSource))]
    internal static async Task HandleAsync_DataverseGetSetResultIsSuccess_ExpectDataverseDeleteCalledOnce(
        ProjectCostSetDeleteIn input,
        DataverseEntitySetGetOut<EmployeeProjectCostJson> dataverseSetGetOut,
        FlatArray<DataverseEntityDeleteIn> expectedInputs)
    {
        var mockDataverseApi = BuildMockDataverseApi<EmployeeProjectCostJson>(dataverseSetGetOut, Result.Success<Unit>(default));
        var handler = new ProjectCostSetDeleteHandler(mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        var actual = await handler.HandleAsync(input, cancellationToken);

        foreach (var expectedInput in expectedInputs)
        {
            mockDataverseApi.Verify(f => f.DeleteEntityAsync(expectedInput, It.IsAny<CancellationToken>()), Times.Once);
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
    [InlineData(DataverseFailureCode.DuplicateRecord, HandlerFailureCode.Transient)]
    [InlineData(DataverseFailureCode.InvalidPayload, HandlerFailureCode.Transient)]
    [InlineData(DataverseFailureCode.InvalidFileSize, HandlerFailureCode.Transient)]
    public static async Task HandleAsync_DataverseDeleteResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, HandlerFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockDataverseApi = BuildMockDataverseApi<EmployeeProjectCostJson>(SomeEmployeeProjectCostJsonOut, dataverseFailure);
        var handler = new ProjectCostSetDeleteHandler(mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ProjectCostDeleteHandlerSource.OutputDeleteTestData), MemberType = typeof(ProjectCostDeleteHandlerSource))]
    internal static async Task HandleAsync_DataverseGetSetResultIsSuccess_ExpectSuccess(
        DataverseEntitySetGetOut<EmployeeProjectCostJson> dataverseSetGetOut, ProjectCostSetDeleteOut expected)
    {
        var mockDataverseApi = BuildMockDataverseApi<EmployeeProjectCostJson>(dataverseSetGetOut, Result.Success<Unit>(default));
        var handler = new ProjectCostSetDeleteHandler(mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);

        Assert.StrictEqual(expected, actual);
    }
}