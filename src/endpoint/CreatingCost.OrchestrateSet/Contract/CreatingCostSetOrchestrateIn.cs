using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct CreatingCostSetOrchestrateIn
{
    [JsonConstructor]
    public CreatingCostSetOrchestrateIn(Guid costPeriodId)
        =>
        CostPeriodId = costPeriodId;

    public Guid CostPeriodId { get; }
}