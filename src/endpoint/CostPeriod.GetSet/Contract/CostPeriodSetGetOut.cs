using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct CostPeriodSetGetOut
{
    [JsonBodyOut]
    public required FlatArray<CostPeriod> Periods { get; init; }
}