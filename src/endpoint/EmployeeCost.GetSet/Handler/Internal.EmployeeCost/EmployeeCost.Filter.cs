using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial record class DbEmployeeCost
{
    internal static DbParameterFilter BuildDefaultFilter(Guid periodId)
        =>
        new($"{AliasName}.gg_period_id", DbFilterOperator.Equal, periodId, "periodId");
}