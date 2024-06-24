using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct ProjectCostSetDeleteIn
{
    [JsonConstructor]
    public ProjectCostSetDeleteIn(Guid costPeriodId, int maxItems)
    {
        CostPeriodId = costPeriodId;
        MaxItems = maxItems;
    }

    public Guid CostPeriodId { get; }

    public int MaxItems { get; }
}