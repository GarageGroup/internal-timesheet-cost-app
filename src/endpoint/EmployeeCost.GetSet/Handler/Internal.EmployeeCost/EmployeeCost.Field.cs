using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

partial record class DbEmployeeCost
{
    [DbSelect(All, AliasName, $"{AliasName}.gg_cost")]
    public decimal Cost { get; init; }

    [DbSelect(All, UserAlias, $"{UserAlias}.{SystemUserIdFieldName}")]
    public Guid UserId { get; init; }
}