using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Cost.Endpoint.CreatingCost.OrchestrateSet.Test")]

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