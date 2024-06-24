using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class OrchestrationAsyncPipeline
{
    internal static AsyncPipeline<FlatArray<TOut>, Failure<HandlerFailureCode>> ForwardParallel<TIn, TOut>(
        this AsyncPipeline<FlatArray<TIn>, Failure<HandlerFailureCode>> pipeline,
        Func<TIn, CancellationToken, Task<Result<TOut, Failure<HandlerFailureCode>>>> forward)
    {
        return pipeline.Forward(InnerForwardAsync);

        async Task<Result<FlatArray<TOut>, Failure<HandlerFailureCode>>> InnerForwardAsync(
            FlatArray<TIn> inputs, CancellationToken cancellationToken)
        {
            if (inputs.IsEmpty)
            {
                return Result.Success<FlatArray<TOut>>(default);
            }

            var builder = FlatArray<TOut>.Builder.OfLength(inputs.Length);
            var index = 0;

            foreach (var chunk in inputs.SplitIntoChunks())
            {
                foreach (var result in await Task.WhenAll(chunk.Select(InnerInvokeAsync)))
                {
                    if (result.IsFailure)
                    {
                        return result.FailureOrThrow();
                    }

                    builder[index++] = result.SuccessOrThrow();
                }
            }

            return builder.MoveToFlatArray();

            Task<Result<TOut, Failure<HandlerFailureCode>>> InnerInvokeAsync(TIn input)
                =>
                forward.Invoke(input, cancellationToken);
        }
    }
}