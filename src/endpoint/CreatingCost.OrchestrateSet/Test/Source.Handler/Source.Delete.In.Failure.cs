using GarageGroup.Infra;
using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.CreatingCost.OrchestrateSet.Test;

using ProjectCostSetDeleteActivityResult = Result<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>, Failure<HandlerFailureCode>>;

internal static partial class CreatingCostOrchestrateHandlerSource
{
    public static TheoryData<FlatArray<ProjectCostSetDeleteActivityResult>, Failure<HandlerFailureCode>> InputDeleteFailureTestData
        =>
        new()
        {
            {
                [
                    Failure.Create(HandlerFailureCode.Transient, "Some failure text", SomeException)
                ],
                Failure.Create(HandlerFailureCode.Transient, "Some failure text", SomeException)
            },
            {
                [
                    new OrchestrationActivityCallOut<ProjectCostSetDeleteOut>(
                        value: new()
                        {
                            HasMore = true
                        }),
                    SomeException.ToFailure(HandlerFailureCode.Persistent, "Some Message")
                ],
                Failure.Create(HandlerFailureCode.Persistent, "Some Message", SomeException)
            },
            {
                [
                    new OrchestrationActivityCallOut<ProjectCostSetDeleteOut>(
                        value: new()
                        {
                            HasMore = true
                        }),
                    new OrchestrationActivityCallOut<ProjectCostSetDeleteOut>(
                        value: new()
                        {
                            HasMore = true
                        }),
                    Failure.Create(HandlerFailureCode.Transient, "Failure Message", SomeException)
                ],
                Failure.Create(HandlerFailureCode.Transient, "Failure Message", SomeException)
            }
        };
}