using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial record class DbTimesheet
{
    private static readonly DbRawFilter PeriodFilter
        =
        new($"{PeriodAliasName}.gg_from_date <= {AliasName}.gg_date AND {PeriodAliasName}.gg_to_date >= {AliasName}.gg_date");

    internal static DbParameterFilter BuildOwnerFilter(Guid ownerId)
        =>
        new($"{AliasName}.ownerid", DbFilterOperator.Equal, ownerId, "ownerId");

    internal static DbExistsFilter BuildDateFilter(Guid periodId)
        =>
        new(
            selectQuery: new(
                tableName: "gg_employee_cost_period",
                tableAlias: PeriodAliasName)
            {
                Top = 1,
                SelectedFields = new("1"),
                Filter = new DbCombinedFilter(DbLogicalOperator.And)
                {
                    Filters =
                    [
                        new DbParameterFilter($"{PeriodAliasName}.gg_employee_cost_periodid", DbFilterOperator.Equal, periodId, "periodId"),
                        PeriodFilter
                    ]
                }
            });
}