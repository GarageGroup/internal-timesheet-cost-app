using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public interface ICreatingCostSetStartHandler : IHandler<CreatingCostSetStartIn, IOrchestrationInstanceId>
{
    public const string FunctionName = "StartCreatingCosts";
}