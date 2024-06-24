using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public interface IProjectCostSetDeleteHandler : IHandler<ProjectCostSetDeleteIn, ProjectCostSetDeleteOut>
{
    public const string FunctionName = "DeleteProjectCosts";
}