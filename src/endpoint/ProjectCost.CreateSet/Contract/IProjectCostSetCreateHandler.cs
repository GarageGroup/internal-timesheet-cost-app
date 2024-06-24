using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public interface IProjectCostSetCreateHandler : IHandler<ProjectCostSetCreateIn, Unit>
{
    public const string FunctionName = "CreateProjectCosts";
}