using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class CostPeriodSetGetFunc
{
    public ValueTask<Result<CostPeriodSetGetOut, Failure<Unit>>> InvokeAsync(
        Unit input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}