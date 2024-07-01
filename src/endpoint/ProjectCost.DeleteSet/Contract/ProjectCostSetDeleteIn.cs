using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct ProjectCostSetDeleteIn
{
    [JsonConstructor]
    public ProjectCostSetDeleteIn(Guid systemUserId, Guid costPeriodId, int maxItems)
    {
        SystemUserId = systemUserId;
        CostPeriodId = costPeriodId;
        MaxItems = maxItems;
    }

    public Guid SystemUserId { get; }

    public Guid CostPeriodId { get; }

    public int MaxItems { get; }
}