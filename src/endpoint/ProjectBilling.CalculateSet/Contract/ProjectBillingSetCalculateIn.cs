using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class ProjectBillingSetCalculateIn
{
    public ProjectBillingSetCalculateIn(Guid callerUserId, Guid billingPeriodId)
    {
        CallerUserId = callerUserId;
        BillingPeriodId = billingPeriodId;
    }

    public Guid CallerUserId { get; }

    public Guid BillingPeriodId { get; }
}