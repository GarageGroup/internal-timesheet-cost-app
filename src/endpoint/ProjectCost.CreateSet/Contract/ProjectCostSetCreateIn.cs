using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class ProjectCostSetCreateIn
{
    public ProjectCostSetCreateIn(Guid costPeriodId, Guid systemUserId, Guid callerUserId, decimal employeeCost)
    {
        CostPeriodId = costPeriodId;
        SystemUserId = systemUserId;
        CallerUserId = callerUserId;
        EmployeeCost = employeeCost;
    }

    public Guid CostPeriodId { get; }

    public Guid SystemUserId { get; }

    public Guid CallerUserId { get; }

    public decimal EmployeeCost { get; }
}