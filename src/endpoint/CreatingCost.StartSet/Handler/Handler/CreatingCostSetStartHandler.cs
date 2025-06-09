using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class CreatingCostSetStartHandler(IOrchestrationInstanceScheduleSupplier orchestrationInstanceApi)
    : ICreatingCostSetStartHandler;