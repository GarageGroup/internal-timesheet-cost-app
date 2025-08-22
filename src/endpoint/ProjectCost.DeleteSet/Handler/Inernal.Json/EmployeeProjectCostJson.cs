using GarageGroup.Infra;
using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal readonly record struct EmployeeProjectCostJson
{
    private const string EntityPluralName
        =
        "gg_employee_project_costs";

    private const string EmployeeProjectCostIdFieldName
        =
        "gg_employee_project_costid";

    internal static DataverseEntityDeleteIn BuildDataverseDeleteInput(Guid id, Guid callerUserId)
        =>
        new(
            entityPluralName: EntityPluralName,
            entityKey: new DataversePrimaryKey(id))
        {
            CallerObjectId = callerUserId
        };

    internal static DataverseEntitySetGetIn BuildDataverseSetGetInput(Guid periodId, int maxPageSize, Guid callerUserId)
        =>
        new(
            entityPluralName: EntityPluralName,
            selectFields: new(EmployeeProjectCostIdFieldName),
            filter: $"_gg_period_id_value eq '{periodId}' and gg_createdmethod_is eq true")
        {
            MaxPageSize = maxPageSize,
            CallerObjectId = callerUserId
        };

    [JsonPropertyName(EmployeeProjectCostIdFieldName)]
    public Guid Id { get; init; }
}