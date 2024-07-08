using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial record class DbProjectCost
{
    [DbSelect(All, AliasName, $"SUM({AliasName}.gg_cost)")]
    public decimal? TotalCost { get; init; }

    [DbSelect(All, AliasName, $"SUM({AliasName}.gg_hours_total)")]
    public decimal? TotalHours { get; init; }
}