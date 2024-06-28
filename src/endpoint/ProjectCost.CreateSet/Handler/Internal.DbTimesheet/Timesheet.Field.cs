using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

partial record class DbTimesheet
{
    [DbSelect(All, AliasName, $"{AliasName}.gg_finproject_id", GroupBy = true)]
    public Guid? ProjectId { get; init; }

    [DbSelect(All, AliasName, $"SUM({AliasName}.gg_duration)")]
    public decimal Duration { get; init; }
}