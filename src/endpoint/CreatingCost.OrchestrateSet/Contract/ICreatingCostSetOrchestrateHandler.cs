using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public interface ICreatingCostSetOrchestrateHandler : IHandler<CreatingCostSetOrchestrateIn, CreatingCostSetOrchestrateOut>
{
    public const string FunctionName = "OrchestrateCreatingCosts";
}