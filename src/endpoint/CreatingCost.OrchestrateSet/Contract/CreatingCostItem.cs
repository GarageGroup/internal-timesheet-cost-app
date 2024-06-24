using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Internal.Timesheet;

public sealed record class CreatingCostItem
{
    public CreatingCostItem([AllowNull] string employeeName, bool isCompleted)
    {
        EmployeeName = employeeName.OrEmpty();
        IsCompleted = isCompleted;
    }

    public string EmployeeName { get; }

    public bool IsCompleted { get; }
}