using Castle.Core.Resource;
using GarageGroup.Infra;
using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.CreatingCost.OrchestrateSet.Test;

internal static partial class CreatingCostOrchestrateHandlerSource
{
    private static readonly Exception sourceException
        =
        new("Some exception message");

    public static TheoryData<FlatArray<Result<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>, Failure<HandlerFailureCode>>>, Failure<HandlerFailureCode>> InputDeleteFailureTestData
        =>
        new()
        {
            {
                [
                    sourceException.ToFailure(HandlerFailureCode.Transient, "Some failure text")
                ],
                Failure.Create(HandlerFailureCode.Transient, "Some failure text", sourceException)
            },
            {
                [
                    Result.Success<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>>(new(
                        value: new()
                        {
                            HasMore = true
                        })),
                    sourceException.ToFailure(HandlerFailureCode.Persistent, "Some failure text")
                ],
                Failure.Create(HandlerFailureCode.Persistent, "Some failure text", sourceException)
            },
            {
                [
                    Result.Success<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>>(new(
                        value: new()
                        {
                            HasMore = true
                        })),
                    Result.Success<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>>(new(
                        value: new()
                        {
                            HasMore = true
                        })),
                    sourceException.ToFailure(HandlerFailureCode.Transient, "Some failure text")
                ],
                Failure.Create(HandlerFailureCode.Transient, "Some failure text", sourceException)
            }
        };
}