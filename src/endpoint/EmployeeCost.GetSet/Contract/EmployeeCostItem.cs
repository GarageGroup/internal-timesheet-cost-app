using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class EmployeeCostItem
{
    public EmployeeCostItem(Guid systemUserId, string? employeeName, decimal employeeCost)
    {
        SystemUserId = systemUserId;
        EmployeeName = employeeName.OrEmpty();
        EmployeeCost = employeeCost;
    }

    public Guid SystemUserId { get; }

    public string EmployeeName { get; }

    public decimal EmployeeCost { get; }
}