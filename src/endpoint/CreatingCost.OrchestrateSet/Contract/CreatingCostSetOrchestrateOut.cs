using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct CreatingCostSetOrchestrateOut
{
    public required FlatArray<CreatingCostItem> EmployeeCosts { get; init; }
}