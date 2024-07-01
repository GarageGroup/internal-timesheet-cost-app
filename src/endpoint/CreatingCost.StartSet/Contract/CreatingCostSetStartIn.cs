using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct CreatingCostSetStartIn
{
    public required Guid CallerUserId { get; init; }

    public required Guid CostPeriodId { get; init; }
}