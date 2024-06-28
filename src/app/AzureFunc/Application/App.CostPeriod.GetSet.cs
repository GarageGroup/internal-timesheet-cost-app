using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [EndpointFunction(ICostPeriodSetGetFunc.FunctionName)]
    internal static Dependency<CostPeriodSetGetEndpoint> UseCostPeriodSetGetEndpoint()
        =>
        UseDataverseApi().UseCostPeriodSetGetEndpoint();
}