using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public interface ICreatingCostSetOrchestrateHandler : IHandler<CreatingCostSetOrchestrateIn, Unit>
{
    public const string FunctionName = "OrchestrateCreatingCosts";
}