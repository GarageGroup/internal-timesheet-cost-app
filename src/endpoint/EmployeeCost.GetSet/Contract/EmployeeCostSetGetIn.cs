using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct EmployeeCostSetGetIn
{
    [JsonConstructor]
    public EmployeeCostSetGetIn(Guid costPeriodId)
        =>
        CostPeriodId = costPeriodId;

    public Guid CostPeriodId { get; }
}