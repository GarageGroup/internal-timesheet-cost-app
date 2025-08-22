using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class ProjectCostSetCreateIn
{
    public ProjectCostSetCreateIn(Guid callerUserId, Guid costPeriodId, Guid systemUserId, decimal employeeCost)
    {
        CallerUserId = callerUserId;
        CostPeriodId = costPeriodId;
        SystemUserId = systemUserId;
        EmployeeCost = employeeCost;
    }

    public Guid CallerUserId { get; }

    public Guid CostPeriodId { get; }

    public Guid SystemUserId { get; }

    public decimal EmployeeCost { get; }
}