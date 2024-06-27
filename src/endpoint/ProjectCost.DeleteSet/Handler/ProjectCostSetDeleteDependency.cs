using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.DeleteSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class ProjectCostSetDeleteDependency
{
    public static Dependency<IProjectCostSetDeleteHandler> UseProjectCostSetDeleteHandler(
        this Dependency<IDataverseApiClient> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<IProjectCostSetDeleteHandler>(CreateHandler);

        static ProjectCostSetDeleteHandler CreateHandler(IDataverseApiClient dataverseApi)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            return new(dataverseApi);
        }
    }
}