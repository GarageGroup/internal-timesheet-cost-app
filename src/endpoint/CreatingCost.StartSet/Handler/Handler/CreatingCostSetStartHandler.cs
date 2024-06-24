using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class CreatingCostSetStartHandler : ICreatingCostSetStartHandler
{
    private readonly IOrchestrationInstanceScheduleSupplier orchestrationInstanceApi;

    internal CreatingCostSetStartHandler(IOrchestrationInstanceScheduleSupplier orchestrationInstanceApi)
        =>
        this.orchestrationInstanceApi = orchestrationInstanceApi;
}