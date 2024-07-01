using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct CreatingCostSetStartIn
{
    [JsonConstructor]
    public CreatingCostSetStartIn(Guid systemUserId, Guid costPeriodId)
    {
        SystemUserId = systemUserId;
        CostPeriodId = costPeriodId;
    }
    
    public Guid SystemUserId { get; }

    public Guid CostPeriodId { get; }
}