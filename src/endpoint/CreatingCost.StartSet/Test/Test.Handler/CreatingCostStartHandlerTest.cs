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
            instanceId: new InnerOrchestrationInstanceId("Some instance id"));

    private static readonly CreatingCostSetStartIn SomeInput
        =
        new(
            costPeriodId: new("dfe086be-9513-48dd-915c-fa1a2c1f6d05"));

    private static Mock<IOrchestrationInstanceScheduleSupplier> BuildMockOrchestrationApi(
        in Result<OrchestrationInstanceScheduleOut, Failure<HandlerFailureCode>> result)
    {
        var mock = new Mock<IOrchestrationInstanceScheduleSupplier>();

        _ = mock
            .Setup(
                static a => a.ScheduleInstanceAsync(
                    It.IsAny<OrchestrationInstanceScheduleIn<CreatingCostSetOrchestrateIn>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }

    private sealed record class InnerOrchestrationInstanceId : IOrchestrationInstanceId
    {
        public InnerOrchestrationInstanceId(string id)
            =>
            Id = id;

        public string Id { get; }

        public bool Equals(IOrchestrationInstanceId? other)
            =>
            Equals((object?)other);
    }
}