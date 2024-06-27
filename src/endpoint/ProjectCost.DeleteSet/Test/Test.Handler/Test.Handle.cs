using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.DeleteSet.Test;

partial class ProjectCostDeleteHandlerTest
{
    [Fact]
    public static async Task HandleAsync_ExpectDataverseGetSetCalledOnce()
    {
        var mockDataverseApi = BuildMockDataverseApi<EmployeeProjectCostJson>(
            SomeEmployeeProjectCostJsonOut, Result.Success<Unit>(default));

        var handler = new ProjectCostSetDeleteHandler(mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        var input = new ProjectCostSetDeleteIn(new("80738293-e49b-4c3f-966d-52afc9964da2"), 10);

        _ = await handler.HandleAsync(input, cancellationToken);

        var expectedInput = new DataverseEntitySetGetIn(
            entityPluralName: "gg_employee_project_costs",
            selectFields: ["gg_employee_project_costid"],
            filter: "_gg_period_id_value eq '80738293-e49b-4c3f-966d-52afc9964da2'",
            expandFields: default,
            orderBy: default,
            top: 10);

        mockDataverseApi.Verify(f => f.GetEntitySetAsync<EmployeeProjectCostJson>(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized)]
    [InlineData(DataverseFailureCode.RecordNotFound)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange)]
    [InlineData(DataverseFailureCode.UserNotEnabled)]
    [InlineData(DataverseFailureCode.PrivilegeDenied)]
    [InlineData(DataverseFailureCode.Throttling)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound)]
    [InlineData(DataverseFailureCode.DuplicateRecord)]
    [InlineData(DataverseFailureCode.InvalidPayload)]
    [InlineData(DataverseFailureCode.InvalidFileSize)]
    public static async Task HandleAsync_DataverseGetSetResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockDataverseApi = BuildMockDataverseApi<EmployeeProjectCostJson>(dataverseFailure, Result.Success<Unit>(default));
        var handler = new ProjectCostSetDeleteHandler(mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Failure.Create(HandlerFailureCode.Transient, "Some failure text", sourceException);

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

        foreach(var expectedInput in expectedInputs)
        {
            mockDataverseApi.Verify(f => f.DeleteEntityAsync(expectedInput, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized)]
    [InlineData(DataverseFailureCode.RecordNotFound)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange)]
    [InlineData(DataverseFailureCode.UserNotEnabled)]
    [InlineData(DataverseFailureCode.PrivilegeDenied)]
    [InlineData(DataverseFailureCode.Throttling)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound)]
    [InlineData(DataverseFailureCode.DuplicateRecord)]
    [InlineData(DataverseFailureCode.InvalidPayload)]
    [InlineData(DataverseFailureCode.InvalidFileSize)]
    public static async Task HandleAsync_DataverseDeleteResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockDataverseApi = BuildMockDataverseApi<EmployeeProjectCostJson>(SomeEmployeeProjectCostJsonOut, dataverseFailure);
        var handler = new ProjectCostSetDeleteHandler(mockDataverseApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Failure.Create(HandlerFailureCode.Transient, "Some failure text", sourceException);

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