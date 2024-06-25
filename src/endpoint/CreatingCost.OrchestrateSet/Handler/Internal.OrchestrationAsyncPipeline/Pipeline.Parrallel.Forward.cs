using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class OrchestrationAsyncPipeline
{
    internal static AsyncPipeline<Unit, Failure<HandlerFailureCode>> ForwardParallel<TIn>(
        this AsyncPipeline<FlatArray<TIn>, Failure<HandlerFailureCode>> pipeline,
        Func<TIn, CancellationToken, Task<Result<Unit, Failure<HandlerFailureCode>>>> forward)
    {
        return pipeline.Forward(InnerForwardAsync);

        async Task<Result<Unit, Failure<HandlerFailureCode>>> InnerForwardAsync(
            FlatArray<TIn> inputs, CancellationToken cancellationToken)
        {
            if (inputs.IsEmpty)
            {
                return Result.Success<Unit>(default);
            }

            foreach (var chunk in inputs.SplitIntoChunks())
            {
                foreach (var result in await Task.WhenAll(chunk.Select(InnerInvokeAsync)))
                {
                    if (result.IsFailure)
                    {
                        return result.FailureOrThrow();
                    }
                }
            }

            return Result.Success<Unit>(default);

            Task<Result<Unit, Failure<HandlerFailureCode>>> InnerInvokeAsync(TIn input)
                =>
                forward.Invoke(input, cancellationToken);
        }
    }
}