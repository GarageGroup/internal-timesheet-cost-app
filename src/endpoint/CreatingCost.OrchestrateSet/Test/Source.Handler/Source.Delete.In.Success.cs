using GarageGroup.Infra;
using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.CreatingCost.OrchestrateSet.Test;

using CostSetDeleteActivityIn = OrchestrationActivityCallIn<ProjectCostSetDeleteIn>;
using CostSetDeleteActivityOut = OrchestrationActivityCallOut<ProjectCostSetDeleteOut>;

partial class CreatingCostOrchestrateHandlerSource
{
    public static TheoryData<CreatingCostSetOrchestrateIn, CostSetDeleteActivityIn, FlatArray<CostSetDeleteActivityOut>> InputDeleteSuccessTestData
        =>
        new()
        {
            {
                new(
                    systemUserId: new("c69b6ee2-51a4-4e07-bfda-9ef6fb0be064"),
                    costPeriodId: new("2b715709-e57c-4ad4-8d34-e00201703a69")),
                new(
                    activityName: "DeleteProjectCosts",
                    value: new(
                        systemUserId: new("c69b6ee2-51a4-4e07-bfda-9ef6fb0be064"),
                        costPeriodId: new("2b715709-e57c-4ad4-8d34-e00201703a69"),
                        maxItems: 32)),
                [
                    new(
                        value: new()
                        {
                            HasMore = false
                        })
                ]
            },
            {
                new(
                    systemUserId: new("d0ba3b6d-87a7-4a7d-ba28-c6aed3acd7e0"),
                    costPeriodId: new("615c15aa-f572-4a68-a621-4f3bc13d2717")),
                new(
                    activityName: "DeleteProjectCosts",
                    value: new(
                        systemUserId: new("d0ba3b6d-87a7-4a7d-ba28-c6aed3acd7e0"),
                        costPeriodId: new("615c15aa-f572-4a68-a621-4f3bc13d2717"),
                        maxItems: 32)),
                [
                    new(
                        value: new()
                        {
                            HasMore = true
                        }),
                    new(
                        value: new()
                        {
                            HasMore = false
                        })
                ]
            },
            {
                new(
                    systemUserId: new("6e67a129-8d10-42f9-ac9e-fdc7039aefb3"),
                    costPeriodId: new("c9c2bd97-2ddf-459f-9c1a-023155eb4cf2")),
                new(
                    activityName: "DeleteProjectCosts",
                    value: new(
                        systemUserId: new("6e67a129-8d10-42f9-ac9e-fdc7039aefb3"),
                        costPeriodId: new("c9c2bd97-2ddf-459f-9c1a-023155eb4cf2"),
                        maxItems: 32)),
                [
                    new(
                        value: new()
                        {
                            HasMore = true
                        }),
                    new(
                        value: new()
                        {
                            HasMore = true
                        }),
                    new(
                        value: new()
                        {
                            HasMore = false
                        })
                ]
            }
        };
}