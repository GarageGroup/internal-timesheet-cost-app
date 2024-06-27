using GarageGroup.Infra;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class EmployeeProjectCostJson
{
    private const string EntityPluralName
        =
        "gg_employee_project_costs";

    internal static DataverseEntityCreateIn<EmployeeProjectCostJson> BuildDataverseCreateInput(EmployeeProjectCostJson item)
        =>
        new(
            entityPluralName: EntityPluralName,
            entityData: item);

    internal static string BuildEmployeeLookupValue(Guid employeeId)
        =>
        $"/systemusers({employeeId:D})";

    internal static string BuildPeriodLookupValue(Guid periodId)
        =>
        $"/gg_employee_cost_periods({periodId:D})";

    internal static Dictionary<string, object> BuildExtensionData(Guid projectId, int projectType)
        =>
        projectType switch
        {
            112 => new() { ["gg_regarding_object_id_incident@odata.bind"] = $"/incidents({projectId:D})" },
            3 => new() { ["gg_regarding_object_id_opportunity@odata.bind"] = $"/opportunities({projectId:D})" },
            4 => new() { ["gg_regarding_object_id_lead@odata.bind"] = $"/leads({projectId:D})" },
            _ => new() { ["gg_regarding_object_id_gg_project@odata.bind"] = $"/gg_projects({projectId:D})" }
        };

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("gg_employee_id@odata.bind")]
    public string? EmployeeLookupValue { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("gg_period_id@odata.bind")]
    public string? PeriodLookupValue { get; init; }

    [JsonPropertyName("gg_cost")]
    public decimal Cost { get; init; }

    [JsonExtensionData]
    public Dictionary<string, object>? ExtensionData { get; init; }
}