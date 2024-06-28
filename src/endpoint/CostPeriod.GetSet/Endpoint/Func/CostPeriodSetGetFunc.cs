using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class CostPeriodSetGetFunc : ICostPeriodSetGetFunc
{
    private readonly IDataverseEntitySetGetSupplier dataverseApi;

    internal CostPeriodSetGetFunc(IDataverseEntitySetGetSupplier dataverseApi)
        =>
        this.dataverseApi = dataverseApi;
}