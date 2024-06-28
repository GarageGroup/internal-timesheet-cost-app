using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Internal.Timesheet;

public sealed record class CostPeriod
{
    public CostPeriod([AllowNull] string name, DateOnly from, DateOnly to)
    {
        Name = name.OrEmpty();
        From = from;
        To = to;
    }

    public string Name { get; }

    public DateOnly From { get; }

    public DateOnly To { get; }
}