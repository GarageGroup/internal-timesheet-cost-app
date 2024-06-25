using GarageGroup.Infra;
using System;
using System.Text.Json.Serialization;

namespace GarageGroup.TestConsoleApp;

internal readonly record struct EmployeeProjectCostJson
{
    private const string EntityPluralName
        =
        "gg_employee_project_costs";

    internal static DataverseEntityDeleteIn BuildDataverseDeleteInput(Guid id)
        =>
        new(
            entityPluralName: EntityPluralName,
            entityKey: new DataversePrimaryKey(id));

    internal static DataverseEntitySetGetIn BuildDataverseSetGetInput(Guid periodId, int top)
        =>
        new(
            entityPluralName: EntityPluralName,
            selectFields: default,
            filter: $"_gg_period_id_value qe '{periodId}'",
            expandFields: default,
            orderBy: default,
            top: top);

    [JsonPropertyName("gg_employee_project_costid")]
    public Guid Id { get; init; }
}