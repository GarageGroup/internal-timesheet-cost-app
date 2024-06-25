using GarageGroup.Infra;
using System;
using System.Text.Json.Serialization;

namespace GarageGroup.TestConsoleApp;

internal readonly record struct EmployeeProjectCostJson
{
    private const string EntityPluralName
        =
        "gg_employee_project_costs";

    private const string EmployeeProjectCostIdFieldName
        =
        "gg_employee_project_costid";

    internal static DataverseEntityDeleteIn BuildDataverseDeleteInput(Guid id)
        =>
        new(
            entityPluralName: EntityPluralName,
            entityKey: new DataversePrimaryKey(id));

    internal static DataverseEntitySetGetIn BuildDataverseSetGetInput(Guid periodId, int top)
        =>
        new(
            entityPluralName: EntityPluralName,
            selectFields: [EmployeeProjectCostIdFieldName],
            filter: $"_gg_period_id_value eq '{periodId}'",
            expandFields: default,
            orderBy: default,
            top: top);

    [JsonPropertyName(EmployeeProjectCostIdFieldName)]
    public Guid Id { get; init; }
}