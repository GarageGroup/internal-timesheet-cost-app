using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

[Endpoint(EndpointMethod.Get, "/costPeriods")]
[EndpointTag("TimesheetCost")]
public interface ICostPeriodSetGetFunc
{
    public const string FunctionName = "GetCostPeriods";

    ValueTask<Result<CostPeriodSetGetOut, Failure<Unit>>> InvokeAsync(
        Unit input, CancellationToken cancellationToken);
}