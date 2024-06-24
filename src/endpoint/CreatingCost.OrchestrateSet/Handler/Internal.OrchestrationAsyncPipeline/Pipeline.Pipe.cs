using System;
using System.Threading;

namespace GarageGroup.Internal.Timesheet;

partial class OrchestrationAsyncPipeline
{
    internal static AsyncPipeline<T> Pipe<T>(T value, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Configure(
            PipelineConfiguration)
        .Pipe(
            value, cancellationToken);
}