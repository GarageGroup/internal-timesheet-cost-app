using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("gg_employee_project_cost", AliasName)]
internal sealed partial record class DbProjectCost : IDbEntity<DbProjectCost>
{
    private const string All = "QueryAll";

    private const string AliasName = "c";
}