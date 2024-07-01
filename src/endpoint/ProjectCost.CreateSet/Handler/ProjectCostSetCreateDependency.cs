using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.CreateSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class ProjectCostSetCreateDependency
{
    public static Dependency<IProjectCostSetCreateHandler> UseProjectCostSetCreateHandler<TSqlApi, TDataverseApi>(
        this Dependency<TSqlApi, TDataverseApi> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
        where TDataverseApi : IDataverseImpersonateSupplier<IDataverseEntityCreateSupplier>
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<IProjectCostSetCreateHandler>(CreateHandler);

        static ProjectCostSetCreateHandler CreateHandler(TSqlApi sqlApi, TDataverseApi dataverseApi)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);
            ArgumentNullException.ThrowIfNull(dataverseApi);

            return new(sqlApi, dataverseApi);
        }
    }
}