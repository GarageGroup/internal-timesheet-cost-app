using System;
using GarageGroup.Infra;
using PrimeFuncPack;

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