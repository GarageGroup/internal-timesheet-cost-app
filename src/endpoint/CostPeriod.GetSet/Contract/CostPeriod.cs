using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Internal.Timesheet;

public sealed record class CostPeriod
{
    public CostPeriod(Guid id, [AllowNull] string name, DateOnly from, DateOnly to)
    {
        Id = id;
        Name = name.OrEmpty();
        From = from;
        To = to;
    }

    public Guid Id { get; }

    public string Name { get; }

    public DateOnly From { get; }

    public DateOnly To { get; }
}