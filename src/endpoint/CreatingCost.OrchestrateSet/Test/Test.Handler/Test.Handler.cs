using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.CreatingCost.OrchestrateSet.Test;

partial class CreatingCostOrchestrateHandlerTest
{
    [Theory]
    [MemberData(nameof(CreatingCostOrchestrateHandlerSource.InputDeleteSuccessTestData), MemberType = typeof(CreatingCostOrchestrateHandlerSource))]
    internal static async Task HandleAsync_ExpectOrchestrationDeleteCalledTwice(
        CreatingCostSetOrchestrateIn input, 
        OrchestrationActivityCallIn<ProjectCostSetDeleteIn> expectedInput, 
        FlatArray<Result<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>, Failure<HandlerFailureCode>>> orchestrationOut)
    {
        var mockOrchestration = BuildMockOrchestrationForDeleteTest(orchestrationOut);
        var handler = new CreatingCostSetOrchestrateHandler(mockOrchestration.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await handler.HandleAsync(input, cancellationToken);

        mockOrchestration.Verify(f => f.CallActivityAsync<ProjectCostSetDeleteIn, ProjectCostSetDeleteOut>(expectedInput, cancellationToken), Times.Exactly(orchestrationOut.Length));
    }

    [Theory]
    [MemberData(nameof(CreatingCostOrchestrateHandlerSource.InputDeleteFailureTestData), MemberType = typeof(CreatingCostOrchestrateHandlerSource))]
    public static async Task HandleAsync_OrchestrationDeleteResultIsFailure_ExpectFailure(
        FlatArray<Result<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>, Failure<HandlerFailureCode>>> orchestrationOut,
        Failure<HandlerFailureCode> expected)
    {
        var mockOrchestration = BuildMockOrchestrationForDeleteTest(orchestrationOut);
        var handler = new CreatingCostSetOrchestrateHandler(mockOrchestration.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        var actual = await handler.HandleAsync(SomeInput, cancellationToken);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_OrchestrationDeleteResultIsSuccess_ExpectOrchestrationSetGetCalledOnce()
    {
        var mockOrchestration = BuildMockOrchestration(SomeDeleteOut, SomeSetGetOut, Result.Success<Unit>(default));
        var handler = new CreatingCostSetOrchestrateHandler(mockOrchestration.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        var input = new CreatingCostSetOrchestrateIn(new("dfe086be-9513-48dd-915c-fa1a2c1f6d05"));
        _ = await handler.HandleAsync(input, cancellationToken);

        var expectedInput = new OrchestrationActivityCallIn<EmployeeCostSetGetIn>(
                activityName: "GetEmployeeCosts",
                value: new(new("dfe086be-9513-48dd-915c-fa1a2c1f6d05")));
        mockOrchestration.Verify(f => f.CallActivityAsync<EmployeeCostSetGetIn, EmployeeCostSetGetOut>(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(HandlerFailureCode.Transient)]
    [InlineData(HandlerFailureCode.Persistent)]
    public static async Task HandleAsync_OrchestrationSetGetResultIsFailure_ExpectFailure(HandlerFailureCode sourceFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var orchestrationFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockOrchestration = BuildMockOrchestration(SomeDeleteOut, orchestrationFailure, Result.Success<Unit>(default));
        var handler = new CreatingCostSetOrchestrateHandler(mockOrchestration.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        var actual = await handler.HandleAsync(SomeInput, cancellationToken);
        var expected = Failure.Create(sourceFailureCode, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(CreatingCostOrchestrateHandlerSource.InputCreateTestData), MemberType = typeof(CreatingCostOrchestrateHandlerSource))]
    internal static async Task HandleAsync_OrchestrationSetGetResultIsSuccess_ExpectOrchestrationCreateCalledOnce(
        CreatingCostSetOrchestrateIn input, OrchestrationActivityCallOut<EmployeeCostSetGetOut> setGetOut, FlatArray<OrchestrationActivityCallIn<ProjectCostSetCreateIn>> expectedInputs)
    {
        var mockOrchestration = BuildMockOrchestration(SomeDeleteOut, setGetOut, Result.Success<Unit>(default));
        var handler = new CreatingCostSetOrchestrateHandler(mockOrchestration.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await handler.HandleAsync(input, cancellationToken);

        foreach(var expectedInput in expectedInputs)
        {
            mockOrchestration.Verify(f => f.CallActivityAsync(expectedInput, cancellationToken), Times.Once);
        }
    }

    [Theory]
    [InlineData(HandlerFailureCode.Transient)]
    [InlineData(HandlerFailureCode.Persistent)]
    public static async Task HandleAsync_OrchestrationCreateResultIsFailure_ExpectFailure(HandlerFailureCode sourceFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var orchestrationFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockOrchestration = BuildMockOrchestration(SomeDeleteOut, SomeSetGetOut, orchestrationFailure);
        var handler = new CreatingCostSetOrchestrateHandler(mockOrchestration.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        var actual = await handler.HandleAsync(SomeInput, cancellationToken);
        var expected = Failure.Create(sourceFailureCode, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_OrchestrationCreateResultIsSuccess_ExpectSuccess()
    {
        var mockOrchestration = BuildMockOrchestration(SomeDeleteOut, SomeSetGetOut, Result.Success<Unit>(default));
        var handler = new CreatingCostSetOrchestrateHandler(mockOrchestration.Object);

        var cancellationToken = new CancellationToken(canceled: false);

        var actual = await handler.HandleAsync(SomeInput, cancellationToken);
        var expected = Result.Success<Unit>(default);
        
        Assert.StrictEqual(expected, actual);
    }
}