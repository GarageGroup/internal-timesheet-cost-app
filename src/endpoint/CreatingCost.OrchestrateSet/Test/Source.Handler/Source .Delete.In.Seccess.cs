﻿using GarageGroup.Infra;
using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.CreatingCost.OrchestrateSet.Test;

internal static partial class CreatingCostOrchestrateHandlerSource
{
    public static TheoryData<CreatingCostSetOrchestrateIn, OrchestrationActivityCallIn<ProjectCostSetDeleteIn>, FlatArray<Result<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>, Failure<HandlerFailureCode>>>> InputDeleteSuccessTestData
        =>
        new()
        {
            {
                new(
                    new("2b715709-e57c-4ad4-8d34-e00201703a69")),
                new(
                    activityName: "DeleteProjectCosts",
                    value: new(new("2b715709-e57c-4ad4-8d34-e00201703a69"), 32)),
                [
                    Result.Success<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>>(new(
                        value: new()
                        {
                            HasMore = false
                        }))
                ]
            },
            {
                new(
                    new("615c15aa-f572-4a68-a621-4f3bc13d2717")),
                new(
                    activityName: "DeleteProjectCosts",
                    value: new(new("615c15aa-f572-4a68-a621-4f3bc13d2717"), 32)),
                [
                    Result.Success<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>>(new(
                        value: new()
                        {
                            HasMore = true
                        })),
                    Result.Success<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>>(new(
                        value: new()
                        {
                            HasMore = false
                        }))
                ]
            },
            {
                new(
                    new("c9c2bd97-2ddf-459f-9c1a-023155eb4cf2")),
                new(
                    activityName: "DeleteProjectCosts",
                    value: new(new("c9c2bd97-2ddf-459f-9c1a-023155eb4cf2"), 32)),
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
                    Result.Success<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>>(new(
                        value: new()
                        {
                            HasMore = false
                        }))
                ]
            }
        };
}