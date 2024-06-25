using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[DbEntity("gg_employee_cost", AliasName)]
[DbJoin(DbJoinType.Inner, "systemuser", UserAlias, $"{AliasName}.gg_employee_id = {UserAlias}.{SystemUserIdFieldName}")]
internal sealed partial record class DbEmployeeCost : IDbEntity<DbEmployeeCost>
{
    private const string All = "QueryAll";

    private const string AliasName = "c";

    private const string UserAlias = "u";

    private const string SystemUserIdFieldName = "systemuserid";
}