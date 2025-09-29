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
        Func<TIn, CancellationToken, Task<Result<Unit, Failure<HandlerFailureCode>>>> second,
        Func<TIn, CancellationToken, Task<Result<TOut, Failure<HandlerFailureCode>>>> third)
    {
        return pipeline.Pipe(InnerPipeAsync);

        async Task<Result<TOut, Failure<HandlerFailureCode>>> InnerPipeAsync(
            TIn input, CancellationToken cancellationToken)
        {
            var firstTask = first.Invoke(input, cancellationToken);
            var secondTask = second.Invoke(input, cancellationToken);
            var thirdTask = third.Invoke(input, cancellationToken);

            await Task.WhenAll(firstTask, secondTask, thirdTask);

            if (firstTask.Result.IsFailure)
            {
                return firstTask.Result.FailureOrThrow();
            }

            if (secondTask.Result.IsFailure)
            {
                return secondTask.Result.FailureOrThrow();
            }

            return thirdTask.Result;
        }
    }
}