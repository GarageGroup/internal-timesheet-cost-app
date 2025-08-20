using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectBilling.EnsureSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class ProjectBillingSetEnsureDependency
{
    public static Dependency<IProjectBillingSetEnsureHandler> UseProjectBillingSetEnsureHandler<TSqlApi, TDataverseApi>(
        this Dependency<TSqlApi, TDataverseApi> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
        where TDataverseApi : IDataverseImpersonateSupplier<IDataverseEntityCreateSupplier>
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<IProjectBillingSetEnsureHandler>(CreateHandler);

        static ProjectBillingSetEnsureHandler CreateHandler(TSqlApi sqlApi, TDataverseApi dataverseApi)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);
            ArgumentNullException.ThrowIfNull(dataverseApi);

            return new(sqlApi, dataverseApi);
        }
    }
}