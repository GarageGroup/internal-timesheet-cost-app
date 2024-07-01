using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct CreatingCostSetOrchestrateIn
{
    [JsonConstructor]
    public CreatingCostSetOrchestrateIn(Guid callerUserId, Guid costPeriodId)
    {
        CallerUserId = callerUserId;
        CostPeriodId = costPeriodId;
    }

    public Guid CallerUserId { get; }

    public Guid CostPeriodId { get; }
}