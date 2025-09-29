using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("gg_project_billing_period", AliasName)]
internal sealed partial record class DbProjectBillingPeriod : IDbEntity<DbProjectBillingPeriod>
{
    private const string All = "QueryAll";

    private const string AliasName = "p";
}