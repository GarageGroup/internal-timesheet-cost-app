using System;

namespace GarageGroup.Internal.Timesheet;

public sealed record class ProjectBillingSetEnsureIn
{
    public ProjectBillingSetEnsureIn(Guid callerUserId, Guid billingPeriodId)
    {
        CallerUserId = callerUserId;
        BillingPeriodId = billingPeriodId;
    }

    public Guid CallerUserId { get; }

    public Guid BillingPeriodId { get; }
}