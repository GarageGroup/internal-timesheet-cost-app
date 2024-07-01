using GarageGroup.Infra;
using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.CreateSet.Test;

using DataverseProjectCostCreateIn = DataverseEntityCreateIn<EmployeeProjectCostJson>;

internal static partial class ProjectCostCreateHandlerSource
{
    public static TheoryData<ProjectCostSetCreateIn, FlatArray<DbTimesheet>, FlatArray<DataverseProjectCostCreateIn>, Guid> InputCreateTestData
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
                [
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            CostShare = 0.1m,
                            Cost = 5m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ProjectLookupValue = "/gg_projects(d1f6db66-e731-423f-a85f-0da3675e7b91)"
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            CostShare = 0.2m,
                            Cost = 10m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ProjectLookupValue = "/gg_projects(6cd8c5b8-9628-493d-b790-bc010ed26367)"
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            CostShare = 0.3m,
                            Cost = 15m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ProjectLookupValue = "/gg_projects(45d271f9-338c-4536-a7cc-48f497485200)"
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            CostShare = 0.4m,
                            Cost = 20m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ProjectLookupValue = null
                        })
                ],
                new("5b25be13-5120-4807-979a-c4f879d547b3")
            }
        };
}