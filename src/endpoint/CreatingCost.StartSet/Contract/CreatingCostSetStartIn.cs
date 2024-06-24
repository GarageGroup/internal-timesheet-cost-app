using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct CreatingCostSetStartIn
{
    [JsonConstructor]
    public CreatingCostSetStartIn(Guid costPeriodId)
        =>
        CostPeriodId = costPeriodId;

    public Guid CostPeriodId { get; }
}