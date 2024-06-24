using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class OrchestrationAsyncPipeline
{
    internal static AsyncPipeline<TOut, Failure<HandlerFailureCode>> PipeParallel<TIn, TOut>(
        this AsyncPipeline<TIn> pipeline,
        Func<TIn, CancellationToken, Task<Result<Unit, Failure<HandlerFailureCode>>>> first,
        Func<TIn, CancellationToken, Task<Result<TOut, Failure<HandlerFailureCode>>>> second)
    {
        return pipeline.Pipe(InnerPipeAsync);

        async Task<Result<TOut, Failure<HandlerFailureCode>>> InnerPipeAsync(
            TIn input, CancellationToken cancellationToken)
        {
            var firstTask = first.Invoke(input, cancellationToken);
            var secondTask = second.Invoke(input, cancellationToken);

            await Task.WhenAll(firstTask, secondTask);

            return secondTask.Result;
        }
    }
}