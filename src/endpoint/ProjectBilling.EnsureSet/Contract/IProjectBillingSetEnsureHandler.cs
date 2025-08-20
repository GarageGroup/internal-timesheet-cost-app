using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public interface IProjectBillingSetEnsureHandler : IHandler<ProjectBillingSetEnsureIn, Unit>
{
    public const string FunctionName = "EnsureProjectBillingPeriods";
}