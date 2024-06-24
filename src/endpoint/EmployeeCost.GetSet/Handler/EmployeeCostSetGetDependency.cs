using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

public static class EmployeeCostSetGetDependency
{
    public static Dependency<IEmployeeCostSetGetHandler> UseEmployeeCostSetGetHandler<TSqlApi>(
        this Dependency<TSqlApi> dependency)
        where TSqlApi : ISqlQueryEntitySetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<IEmployeeCostSetGetHandler>(CreateHandler);

        static EmployeeCostSetGetHandler CreateHandler(TSqlApi sqlApi)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);
            return new(sqlApi);
        }
    }
}