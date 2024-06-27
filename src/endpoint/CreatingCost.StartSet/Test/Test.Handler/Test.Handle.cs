using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.StartSet.OrchestrateSet.Test;

partial class CreatingCostStartHandlerTest
{
    [Fact]
    public static async Task HandleAsync_ExpectOrchestrationCalledOnce()
    {
        var mockOrchestrationApi = BuildMockOrchestrationApi(SomeOrchestrationOut);
        var handler = new CreatingCostSetStartHandler(mockOrchestrationApi.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        var input = new CreatingCostSetStartIn(new("dfe086be-9513-48dd-915c-fa1a2c1f6d05"));

        _ = await handler.HandleAsync(input, cancellationToken);

        var expectedInput = new OrchestrationInstanceScheduleIn<CreatingCostSetOrchestrateIn>(
            orchestratorName: "OrchestrateCreatingCosts",
            value: new(
                costPeriodId: new("dfe086be-9513-48dd-915c-fa1a2c1f6d05")));

        mockOrchestrationApi.Verify(f => f.ScheduleInstanceAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(HandlerFailureCode.Transient)]
    [InlineData(HandlerFailureCode.Persistent)]
    public static async Task HandleAsync_OrchestrationResultIsFailure_ExpectFailure(
        HandlerFailureCode sourceFailureCode)
    {
        var sourceException = new Exception("Some exception message");
        var orchestrationFailure = sourceException.ToFailure(sourceFailureCode, "Some failure text");

        var mockOrchestrationApi = BuildMockOrchestrationApi(orchestrationFailure);
        var handler = new CreatingCostSetStartHandler(mockOrchestrationApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Failure.Create(sourceFailureCode, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_OrchestrationResultIsSuccess_ExpectSuccess()
    {
        var instanceId = new InnerOrchestrationInstanceId("InstanceId");

        var orchestrationInstanceScheduleOut = new OrchestrationInstanceScheduleOut(instanceId);
        var mockOrchestrationApi = BuildMockOrchestrationApi(orchestrationInstanceScheduleOut);

        var handler = new CreatingCostSetStartHandler(mockOrchestrationApi.Object);
        var actual = await handler.HandleAsync(SomeInput, default);

        Assert.Equal(instanceId, actual);
    }
}