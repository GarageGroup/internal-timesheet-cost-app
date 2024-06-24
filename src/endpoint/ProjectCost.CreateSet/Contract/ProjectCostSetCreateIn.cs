using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class ProjectCostSetCreateIn
{
    public ProjectCostSetCreateIn(Guid costPeriodId, Guid systemUserId, decimal employeeCost)
    {
        CostPeriodId = costPeriodId;
        SystemUserId = systemUserId;
        EmployeeCost = employeeCost;
    }

    public Guid CostPeriodId { get; }

    public Guid SystemUserId { get; }

    public decimal EmployeeCost { get; }
}