using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectBilling.CalculateSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class ProjectBillingSetCalculateDependency
{
    public static Dependency<IProjectBillingSetCalculateHandler> UseProjectBillingSetCalculateHandler<TSqlApi>(
        this Dependency<TSqlApi, IHttpApi> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<IProjectBillingSetCalculateHandler>(CreateHandler);

        static ProjectBillingSetCalculateHandler CreateHandler(TSqlApi sqlApi, IHttpApi httpApi)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);
            ArgumentNullException.ThrowIfNull(httpApi);

            return new(sqlApi, httpApi);
        }
    }
}