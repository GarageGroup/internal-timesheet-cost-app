using GarageGroup.Infra;
using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.DeleteSet.Test;

internal static partial class ProjectCostDeleteHandlerSource
{
    public static TheoryData<ProjectCostSetDeleteIn, DataverseEntitySetGetOut<EmployeeProjectCostJson>, FlatArray<DataverseEntityDeleteIn>, ProjectCostSetDeleteOut> InputDeleteTestData
        =>
        new()
        {
            {
                new(
                    costPeriodId: new("80738293-e49b-4c3f-966d-52afc9964da2"),
                    maxItems: 2),
                new(
                    value:
                    [
                        new()
                        {
                            Id = new("98afb8da-f195-4736-a72f-7b61b268624e")
                        },
                        new()
                        {
                            Id = new("7acffba8-7caa-4c7e-86c6-874ec9550849"),
                        }
                    ]),
                [
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityKey: new DataversePrimaryKey(new("98afb8da-f195-4736-a72f-7b61b268624e"))),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityKey: new DataversePrimaryKey(new("7acffba8-7caa-4c7e-86c6-874ec9550849")))
                ],
                new()
                {
                    HasMore = false
                }
            },
            {
                new(
                    costPeriodId: new("80738293-e49b-4c3f-966d-52afc9964da2"),
                    maxItems: 2),
                new(
                    value:
                    [
                        new()
                        {
                            Id = new("98afb8da-f195-4736-a72f-7b61b268624e")
                        },
                        new()
                        {
                            Id = new("7acffba8-7caa-4c7e-86c6-874ec9550849"),
                        }
                    ], 
                    nextLink: "NextLink"),
                [
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityKey: new DataversePrimaryKey(new("98afb8da-f195-4736-a72f-7b61b268624e"))),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityKey: new DataversePrimaryKey(new("7acffba8-7caa-4c7e-86c6-874ec9550849")))
                ],
                new()
                {
                    HasMore = true
                }
            }
        };
}