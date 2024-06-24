using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct EmployeeCostSetGetOut
{
    public required FlatArray<EmployeeCostItem> EmployeeCostItems { get; init; }
}