using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial record class DbProjectBillingPeriod
{
    [DbSelect(All, AliasName, $"{AliasName}.gg_project_billing_periodid")]
    public Guid Id { get; init; }
}