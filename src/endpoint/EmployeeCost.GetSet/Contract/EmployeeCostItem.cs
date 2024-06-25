using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class EmployeeCostItem
{
    public EmployeeCostItem(Guid systemUserId, decimal employeeCost)
    {
        SystemUserId = systemUserId;
        EmployeeCost = employeeCost;
    }

    public Guid SystemUserId { get; }

    public decimal EmployeeCost { get; }
}