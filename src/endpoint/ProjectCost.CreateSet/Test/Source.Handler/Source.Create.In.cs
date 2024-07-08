using GarageGroup.Infra;
using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.CreateSet.Test;

using CostCraeteIn = DataverseEntityCreateIn<EmployeeProjectCostJson>;

internal static partial class ProjectCostCreateHandlerSource
{
    public static TheoryData<ProjectCostSetCreateIn, FlatArray<DbTimesheet>, FlatArray<DbProjectCost>, FlatArray<CostCraeteIn>> InputCreateTestData
        =>
        new()
        {
            {
                new(
                    costPeriodId: new("a03eb221-654e-4e80-8054-c489d04ef3e2"),
                    systemUserId: new("fd7c47d1-bc37-418d-b2fd-9546ce03aa9a"),
                    callerUserId: new("5b25be13-5120-4807-979a-c4f879d547b3"),
                    employeeCost: 50m),
                default,
                default,
                default
            },
            {
                new(
                    costPeriodId: new("a03eb221-654e-4e80-8054-c489d04ef3e2"),
                    systemUserId: new("fd7c47d1-bc37-418d-b2fd-9546ce03aa9a"),
                    callerUserId: new("5b25be13-5120-4807-979a-c4f879d547b3"),
                    employeeCost: 50m),
                [
                    new()
                    {
                        ProjectId = new("d1f6db66-e731-423f-a85f-0da3675e7b91"),
                        Duration = 2m
                    },
                    new()
                    {
                        ProjectId = new("6cd8c5b8-9628-493d-b790-bc010ed26367"),
                        Duration = 4m
                    },
                    new()
                    {
                        ProjectId = new("45d271f9-338c-4536-a7cc-48f497485200"),
                        Duration = 6m
                    },
                    new()
                    {
                        ProjectId = null,
                        Duration = 8m
                    }
                ],
                default,
                [
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            CostShare = 0.1m,
                            Cost = 5m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ProjectLookupValue = "/gg_projects(d1f6db66-e731-423f-a85f-0da3675e7b91)",
                            HoursTotal = 2m
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            CostShare = 0.2m,
                            Cost = 10m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ProjectLookupValue = "/gg_projects(6cd8c5b8-9628-493d-b790-bc010ed26367)",
                            HoursTotal = 4m
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            CostShare = 0.3m,
                            Cost = 15m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ProjectLookupValue = "/gg_projects(45d271f9-338c-4536-a7cc-48f497485200)",
                            HoursTotal = 6m
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            CostShare = 0.4m,
                            Cost = 20m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ProjectLookupValue = null,
                            HoursTotal = 8m
                        })
                ]
            },
            {
                new(
                    costPeriodId: new("ebd4e85a-05d8-4c4f-a35c-10b7751b776b"),
                    systemUserId: new("8bdb736b-97e0-4946-9317-9a2162601c96"),
                    callerUserId: new("a06e8898-ab21-48ee-8994-8786a258ed44"),
                    employeeCost: 200.5m),
                [
                    new()
                    {
                        ProjectId = new("a6594bc4-be6f-41b1-8544-822739b77f37"),
                        Duration = 62.6m
                    },
                    new()
                    {
                        ProjectId = new("6cd8c5b8-9628-493d-b790-bc010ed26367"),
                        Duration = 187.8m
                    }
                ],
                [
                    new()
                    {
                        TotalCost = default
                    }
                ],
                [
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            EmployeeLookupValue = "/systemusers(8bdb736b-97e0-4946-9317-9a2162601c96)",
                            PeriodLookupValue = "/gg_employee_cost_periods(ebd4e85a-05d8-4c4f-a35c-10b7751b776b)",
                            ProjectLookupValue = "/gg_projects(a6594bc4-be6f-41b1-8544-822739b77f37)",
                            CostShare = 0.25m,
                            Cost = 50.125m,
                            HoursTotal = 62.6m
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            EmployeeLookupValue = "/systemusers(8bdb736b-97e0-4946-9317-9a2162601c96)",
                            PeriodLookupValue = "/gg_employee_cost_periods(ebd4e85a-05d8-4c4f-a35c-10b7751b776b)",
                            ProjectLookupValue = "/gg_projects(6cd8c5b8-9628-493d-b790-bc010ed26367)",
                            CostShare = 0.75m,
                            Cost = 150.375m,
                            HoursTotal = 187.8m
                        })
                ]
            },
            {
                new(
                    costPeriodId: new("1e93933d-1b0f-4f34-9cf2-42651df9d5e0"),
                    systemUserId: new("09f83351-7c17-4f3f-9178-00f37e62ed48"),
                    callerUserId: new("d914519f-62bb-47b3-bb6c-3c2d21ff9a75"),
                    employeeCost: 200.5m),
                [
                    new()
                    {
                        ProjectId = new("2312b48f-1d3b-4c12-b46f-3da3d7b701f3"),
                        Duration = 62.6m
                    },
                    new()
                    {
                        ProjectId = new("32a5638c-04a8-4154-a402-898bcbba795f"),
                        Duration = 187.8m
                    }
                ],
                [
                    new()
                    {
                        TotalCost = 20.5m
                    }
                ],
                [
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            EmployeeLookupValue = "/systemusers(09f83351-7c17-4f3f-9178-00f37e62ed48)",
                            PeriodLookupValue = "/gg_employee_cost_periods(1e93933d-1b0f-4f34-9cf2-42651df9d5e0)",
                            ProjectLookupValue = "/gg_projects(2312b48f-1d3b-4c12-b46f-3da3d7b701f3)",
                            CostShare = 0.25m,
                            Cost = 45,
                            HoursTotal = 62.6m
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            EmployeeLookupValue = "/systemusers(09f83351-7c17-4f3f-9178-00f37e62ed48)",
                            PeriodLookupValue = "/gg_employee_cost_periods(1e93933d-1b0f-4f34-9cf2-42651df9d5e0)",
                            ProjectLookupValue = "/gg_projects(32a5638c-04a8-4154-a402-898bcbba795f)",
                            CostShare = 0.75m,
                            Cost = 135,
                            HoursTotal = 187.8m
                        })
                ]
            },
            {
                new(
                    costPeriodId: new("1e93933d-1b0f-4f34-9cf2-42651df9d5e0"),
                    systemUserId: new("09f83351-7c17-4f3f-9178-00f37e62ed48"),
                    callerUserId: new("d914519f-62bb-47b3-bb6c-3c2d21ff9a75"),
                    employeeCost: 200.5m),
                [
                    new()
                    {
                        ProjectId = new("2312b48f-1d3b-4c12-b46f-3da3d7b701f3"),
                        Duration = 50.08m
                    },
                    new()
                    {
                        ProjectId = new("32a5638c-04a8-4154-a402-898bcbba795f"),
                        Duration = 175.28m
                    }
                ],
                [
                    new()
                    {
                        TotalCost = 20.5m,
                        TotalHours = 25.04m
                    },
                    new()
                    {
                        TotalCost = 35.7m,
                        TotalHours = 250
                    }
                ],
                [
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            EmployeeLookupValue = "/systemusers(09f83351-7c17-4f3f-9178-00f37e62ed48)",
                            PeriodLookupValue = "/gg_employee_cost_periods(1e93933d-1b0f-4f34-9cf2-42651df9d5e0)",
                            ProjectLookupValue = "/gg_projects(2312b48f-1d3b-4c12-b46f-3da3d7b701f3)",
                            CostShare = 0.2m,
                            Cost = 36,
                            HoursTotal = 50.08m
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            EmployeeLookupValue = "/systemusers(09f83351-7c17-4f3f-9178-00f37e62ed48)",
                            PeriodLookupValue = "/gg_employee_cost_periods(1e93933d-1b0f-4f34-9cf2-42651df9d5e0)",
                            ProjectLookupValue = "/gg_projects(32a5638c-04a8-4154-a402-898bcbba795f)",
                            CostShare = 0.7m,
                            Cost = 126,
                            HoursTotal = 175.28m
                        })
                ]
            }
        };
}