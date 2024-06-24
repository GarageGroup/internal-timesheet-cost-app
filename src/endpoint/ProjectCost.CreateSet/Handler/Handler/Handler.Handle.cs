using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class ProjectCostSetCreateHandler
{
    public ValueTask<Result<Unit, Failure<HandlerFailureCode>>> HandleAsync(
        ProjectCostSetCreateIn? input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            Random.Shared.Next(100, 1000), cancellationToken)
        .On(
            Task.Delay)
        .Pipe(
            _ => Result.Success<Unit>(default).With<Failure<HandlerFailureCode>>());
}