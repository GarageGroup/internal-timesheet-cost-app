using GarageGroup.Infra;
using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.DeleteSet.Test;

using DataverseProjectCostSetGetOut = DataverseEntitySetGetOut<EmployeeProjectCostJson>;

partial class ProjectCostDeleteHandlerSource
{
    public static TheoryData<ProjectCostSetDeleteIn, DataverseProjectCostSetGetOut, FlatArray<DataverseEntityDeleteIn>, Guid> InputDeleteTestData
        =>
        new()
        {
            {
                new(
                    callerUserId: new("b01c0c4b-484d-4af9-9b62-ed40c2eeb26c"),
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
                new("b01c0c4b-484d-4af9-9b62-ed40c2eeb26c")
            },
            {
                new(
                    callerUserId: new("02db1d32-bf73-4b1b-8c1d-fc69606f50cf"),
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
                new("02db1d32-bf73-4b1b-8c1d-fc69606f50cf")
            }
        };
}