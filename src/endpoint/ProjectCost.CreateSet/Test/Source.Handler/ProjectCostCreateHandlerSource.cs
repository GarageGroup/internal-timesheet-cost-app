namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.CreateSet.Test;

internal static partial class ProjectCostCreateHandlerSource
{
    private static string InnerBuildProjectBillingPeriodLookupValue(string periodId, string projectId)
        =>
        $"/gg_project_billing_periods(_gg_billingperiod_id_value={periodId},_gg_project_id_value={projectId})";
}