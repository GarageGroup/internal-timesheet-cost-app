using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.StartSet.OrchestrateSet.Test;

public static partial class CreatingCostStartHandlerTest
{
    private static readonly OrchestrationInstanceScheduleOut SomeOrchestrationOut
        =
        new(
            instanceId: BuildOrchestrationInstanceId("Some instance id"));

    private static readonly CreatingCostSetStartIn SomeInput
        =
        new(
            new("dfe086be-9513-48dd-915c-fa1a2c1f6d05"));

    private static Mock<IOrchestrationInstanceScheduleSupplier> BuildMockOrchestration<TIn>(
        in Result<OrchestrationInstanceScheduleOut, Failure<HandlerFailureCode>> result)
    {
        var mock = new Mock<IOrchestrationInstanceScheduleSupplier>();

        _ = mock
            .Setup(static a => a.ScheduleInstanceAsync(It.IsAny<OrchestrationInstanceScheduleIn<TIn>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }

    private static IOrchestrationInstanceId BuildOrchestrationInstanceId(
        string instanceId)
        =>
        Mock.Of<IOrchestrationInstanceId>(a => a.Id == instanceId);
}