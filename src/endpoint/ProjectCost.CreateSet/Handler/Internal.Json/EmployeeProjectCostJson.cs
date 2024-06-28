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

    internal static string? BuildProjectLookupValue(Guid? projectId)
        => projectId is not null ? $"/gg_projects({projectId:D})" : null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("gg_employee_id@odata.bind")]
    public string? EmployeeLookupValue { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("gg_period_id@odata.bind")]
    public string? PeriodLookupValue { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("gg_finproject_id@odata.bind")]
    public string? ProjectLookupValue { get; init; }

    [JsonPropertyName("gg_cost_share")]
    public decimal CostShare { get; init; }

    [JsonPropertyName("gg_cost")]
    public decimal Cost { get; init; }
}