using GarageGroup.Infra;
using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.CreateSet.Test;

using DataverseProjectCostCreateIn = DataverseEntityCreateIn<EmployeeProjectCostJson>;

internal static partial class ProjectCostCreateHandlerSource
{
    public static TheoryData<ProjectCostSetCreateIn, FlatArray<DbTimesheet>, FlatArray<DataverseProjectCostCreateIn>> InputCreateTestData
        =>
        new()
        {
            {
                new(
                    costPeriodId: new("a03eb221-654e-4e80-8054-c489d04ef3e2"),
                    systemUserId: new("fd7c47d1-bc37-418d-b2fd-9546ce03aa9a"),
                    employeeCost: 50m),
                default,
                default
            },
            {
                new(
                    costPeriodId: new("a03eb221-654e-4e80-8054-c489d04ef3e2"),
                    systemUserId: new("fd7c47d1-bc37-418d-b2fd-9546ce03aa9a"),
                    employeeCost: 50m),
                [
                    new()
                    {
                        ProjectId = new("d1f6db66-e731-423f-a85f-0da3675e7b91"),
                        RegardingObjectTypeCode = 112,
                        Duration = 2m
                    },
                    new()
                    {
                        ProjectId = new("6cd8c5b8-9628-493d-b790-bc010ed26367"),
                        RegardingObjectTypeCode = 4,
                        Duration = 4m
                    },
                    new()
                    {
                        ProjectId = new("45d271f9-338c-4536-a7cc-48f497485200"),
                        RegardingObjectTypeCode = 3,
                        Duration = 6m
                    },
                    new()
                    {
                        ProjectId = new("2c38cfc6-b832-4be8-a0f0-eec9390c1fb2"),
                        RegardingObjectTypeCode = 10912,
                        Duration = 8m
                    }
                ],
                [
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            Cost = 5m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ExtensionData = new()
                            {
                                ["gg_regarding_object_id_incident@odata.bind"] = "/incidents(d1f6db66-e731-423f-a85f-0da3675e7b91)"
                            }
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            Cost = 10m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ExtensionData = new()
                            {
                                ["gg_regarding_object_id_lead@odata.bind"] = "/leads(6cd8c5b8-9628-493d-b790-bc010ed26367)"
                            }
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            Cost = 15m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ExtensionData = new()
                            {
                                ["gg_regarding_object_id_opportunity@odata.bind"] = "/opportunities(45d271f9-338c-4536-a7cc-48f497485200)"
                            }
                        }),
                    new(
                        entityPluralName: "gg_employee_project_costs",
                        entityData: new()
                        {
                            Cost = 20m,
                            EmployeeLookupValue = "/systemusers(fd7c47d1-bc37-418d-b2fd-9546ce03aa9a)",
                            PeriodLookupValue = "/gg_employee_cost_periods(a03eb221-654e-4e80-8054-c489d04ef3e2)",
                            ExtensionData = new()
                            {
                                ["gg_regarding_object_id_gg_project@odata.bind"] = "/gg_projects(2c38cfc6-b832-4be8-a0f0-eec9390c1fb2)"
                            }
                        })
                ]
            }
        };
}