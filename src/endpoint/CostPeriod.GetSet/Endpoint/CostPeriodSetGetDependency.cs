using System;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Internal.Timesheet.Cost.Endpoint.CostPeriod.GetSet.Test")]

namespace GarageGroup.Internal.Timesheet;

public static class CostPeriodSetGetDependency
{
    public static Dependency<CostPeriodSetGetEndpoint> UseCostPeriodSetGetEndpoint<TDataverseApi>(
        this Dependency<TDataverseApi> dependency)
        where TDataverseApi : IDataverseEntitySetGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(CostPeriodSetGetEndpoint.Resolve);

        static CostPeriodSetGetFunc CreateFunc(TDataverseApi dataverseApi)
        {
            ArgumentNullException.ThrowIfNull(dataverseApi);
            return new(dataverseApi);
        }
    }
}