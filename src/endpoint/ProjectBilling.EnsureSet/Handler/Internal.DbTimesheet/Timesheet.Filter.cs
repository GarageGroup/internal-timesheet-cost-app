using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial record class DbTimesheet
{
    internal static readonly DbRawFilter ProjectNotNullFiler
        =
        new($"{AliasName}.gg_finproject_id IS NOT NULL");

    private static readonly DbRawFilter PeriodFilter
        =
        new($"{PeriodAliasName}.gg_from_date <= {AliasName}.gg_date AND {PeriodAliasName}.gg_to_date >= {AliasName}.gg_date");

    private static readonly DbRawFilter BillingPeriodProjectFilter
        =
        new($"{BillingPeriodAliasName}.gg_project_id = {AliasName}.gg_finproject_id");

    internal static DbNotExistsFilter BuildBillingPeriodFilter(Guid periodId)
        =>
        new(
            selectQuery: new("gg_project_billing_period", BillingPeriodAliasName)
            {
                Top = 1,
                SelectedFields = new("1"),
                Filter = new DbCombinedFilter(DbLogicalOperator.And)
                {
                    Filters =
                    [
                        BillingPeriodProjectFilter,
                        new DbParameterFilter($"{BillingPeriodAliasName}.gg_billingperiod_id", DbFilterOperator.Equal, periodId, "periodId")
                    ]
                }
            });

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