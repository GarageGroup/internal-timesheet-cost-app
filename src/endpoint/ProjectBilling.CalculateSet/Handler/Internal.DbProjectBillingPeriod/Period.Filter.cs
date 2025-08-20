using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial record class DbProjectBillingPeriod
{
    internal static DbParameterFilter BuildBillingPeriodFilter(Guid billingPeriodId)
        =>
        new($"{AliasName}.gg_billingperiod_id", DbFilterOperator.Equal, billingPeriodId, "billingPeriodId");
}