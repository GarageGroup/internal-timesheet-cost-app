using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial record class DbProjectCost
{
    internal static readonly DbRawFilter ManualCreationFilter
        =
        new($"{AliasName}.createdonbehalfby IS NULL");

    internal static DbParameterFilter BuildEmployeeIdFilter(Guid employeeId)
        =>
        new(
            fieldName: $"{AliasName}.gg_employee_id",
            @operator: DbFilterOperator.Equal,
            fieldValue: employeeId,
            parameterName: "employeeId");

    internal static DbParameterFilter BuildPeriodIdFilter(Guid periodId)
        =>
        new(
            fieldName: $"{AliasName}.gg_period_id",
            @operator: DbFilterOperator.Equal,
            fieldValue: periodId,
            parameterName: "periodId");
}