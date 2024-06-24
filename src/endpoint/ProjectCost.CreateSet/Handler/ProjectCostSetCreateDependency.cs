using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

public static class ProjectCostSetCreateDependency
{
    public static Dependency<IProjectCostSetCreateHandler> UseProjectCostSetCreateHandler<TSqlApi, TDataverseApi>(
        this Dependency<TSqlApi, TDataverseApi> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
        where TDataverseApi : IDataverseEntityCreateSupplier
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