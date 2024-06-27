using GarageGroup.Infra;
using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.CreatingCost.OrchestrateSet.Test;

using EmployeeCostSetGetActivityOut = OrchestrationActivityCallOut<EmployeeCostSetGetOut>;
using ProjectCostSetCreateActivityIn = FlatArray<OrchestrationActivityCallIn<ProjectCostSetCreateIn>>;

internal static partial class CreatingCostOrchestrateHandlerSource
{
    public static TheoryData<CreatingCostSetOrchestrateIn, EmployeeCostSetGetActivityOut, ProjectCostSetCreateActivityIn> InputCreateTestData
        =>
        new()
        {
            {
                new(
                    new("6dc97930-e8de-4f09-a7f1-8bd09286a7e9")),
                new(
                    value: new()
                    {
                        EmployeeCostItems =
                        [
                            new(
                                systemUserId: new("221c375c-b238-44c1-b2bf-2ebffbea72b7"),
                                employeeCost: 110m),
                            new(
                                systemUserId: new("d0efecda-d6bb-4bfe-a345-b68f24d82a00"),
                                employeeCost: 220m)
                        ]
                    }),
                [
                    new(
                        activityName: "CreateProjectCosts",
                        value: new(
                            costPeriodId: new("6dc97930-e8de-4f09-a7f1-8bd09286a7e9"),
                            systemUserId: new("221c375c-b238-44c1-b2bf-2ebffbea72b7"),
                            employeeCost: 110m)),
                    new(
                        activityName: "CreateProjectCosts",
                        value: new(
                            costPeriodId: new("6dc97930-e8de-4f09-a7f1-8bd09286a7e9"),
                            systemUserId: new("d0efecda-d6bb-4bfe-a345-b68f24d82a00"),
                            employeeCost: 220m)),
                ]
            }
        };
}