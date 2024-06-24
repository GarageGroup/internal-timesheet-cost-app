using System;
using System.Collections.Generic;

namespace GarageGroup.Internal.Timesheet;

internal static partial class OrchestrationAsyncPipeline
{
    private const int ChunkSize = 4;

    private static readonly AsyncPipelineConfiguration PipelineConfiguration;

    static OrchestrationAsyncPipeline()
        =>
        PipelineConfiguration = new()
        {
            ContinueOnCapturedContext = true
        };

    private static IEnumerable<IEnumerable<T>> SplitIntoChunks<T>(this FlatArray<T> items)
    {
        for (int i = 0; i < items.Length; i += ChunkSize)
        {
            yield return InnerGetChunk(i);
        }

        IEnumerable<T> InnerGetChunk(int startIndex)
        {
            for (var i = startIndex; i < startIndex + ChunkSize && i < items.Length; i++)
            {
                yield return items[i];
            }
        }
    }
}