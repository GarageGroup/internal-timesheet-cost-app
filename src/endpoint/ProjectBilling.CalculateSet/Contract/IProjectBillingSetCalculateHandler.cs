using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public interface IProjectBillingSetCalculateHandler : IHandler<ProjectBillingSetCalculateIn, Unit>
{
    public const string FunctionName = "CalculateProjectBillingPeriods";
}