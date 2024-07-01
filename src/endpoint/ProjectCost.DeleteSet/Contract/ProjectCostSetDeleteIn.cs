using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct ProjectCostSetDeleteIn
{
    [JsonConstructor]
    public ProjectCostSetDeleteIn(Guid callerUserId, Guid costPeriodId, int maxItems)
    {
        CallerUserId = callerUserId;
        CostPeriodId = costPeriodId;
        MaxItems = maxItems;
    }

    public Guid CallerUserId { get; }

    public Guid CostPeriodId { get; }

    public int MaxItems { get; }
}