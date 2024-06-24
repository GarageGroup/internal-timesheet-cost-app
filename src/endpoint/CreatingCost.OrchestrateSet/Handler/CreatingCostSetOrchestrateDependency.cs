using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

public static class CreatingCostSetOrchestrateDependency
{
    public static Dependency<ICreatingCostSetOrchestrateHandler> UseCreatingCostSetOrchestrateHandler(
        this Dependency<IOrchestrationActivityApi> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<ICreatingCostSetOrchestrateHandler>(CreateHandler);

        static CreatingCostSetOrchestrateHandler CreateHandler(IOrchestrationActivityApi orchestrationActivityApi)
        {
            ArgumentNullException.ThrowIfNull(orchestrationActivityApi);
            return new(orchestrationActivityApi);
        }
    }
}