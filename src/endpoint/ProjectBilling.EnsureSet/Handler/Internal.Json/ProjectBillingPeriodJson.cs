using System;
using System.Text.Json.Serialization;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class ProjectBillingPeriodJson
{
    private const string EntityPluralName
        =
        "gg_project_billing_periods";

    internal static DataverseEntityCreateIn<ProjectBillingPeriodJson> BuildDataverseCreateInput(ProjectBillingPeriodJson item)
        =>
        new(
            entityPluralName: EntityPluralName,
            entityData: item);

    internal static string BuildPeriodLookupValue(Guid periodId)
        =>
        $"/gg_employee_cost_periods({periodId:D})";

    internal static string BuildProjectLookupValue(Guid projectId)
        =>
        $"/gg_projects({projectId:D})";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("gg_billingperiod_id@odata.bind")]
    public required string PeriodLookupValue { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("gg_project_id@odata.bind")]
    public required string ProjectLookupValue { get; init; }
}