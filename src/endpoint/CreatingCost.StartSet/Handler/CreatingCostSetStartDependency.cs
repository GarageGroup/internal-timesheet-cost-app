using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Cost.Endpoint.StartSet.OrchestrateSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class CreatingCostSetStartDependency
{
    public static Dependency<ICreatingCostSetStartHandler> UseCreatingCostSetStartHandler<TOrchestrationInstanceApi>(
        this Dependency<TOrchestrationInstanceApi> dependency)
        where TOrchestrationInstanceApi : IOrchestrationInstanceScheduleSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<ICreatingCostSetStartHandler>(CreateHandler);

        static CreatingCostSetStartHandler CreateHandler(TOrchestrationInstanceApi orchestrationInstanceApi)
        {
            ArgumentNullException.ThrowIfNull(orchestrationInstanceApi);
            return new(orchestrationInstanceApi);
        }
    }
}