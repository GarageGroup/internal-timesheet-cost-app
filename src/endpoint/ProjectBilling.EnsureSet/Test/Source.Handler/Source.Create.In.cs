using System;
using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectBilling.EnsureSet.Test;

using ProjectBillingCreateIn = DataverseEntityCreateIn<ProjectBillingPeriodJson>;

partial class ProjectBillingSetEnsureHandlerSource
{
    public static TheoryData<ProjectBillingSetEnsureIn, FlatArray<DbTimesheet>, FlatArray<ProjectBillingCreateIn>> InputCreateTestData
        =>
        new()
        {
            {
                new(
                    callerUserId: new("1f18daca-e986-45f2-97c7-171cb3d433aa"),
                    billingPeriodId: new("dc17c040-ab1b-4ad6-933f-4119c7bede32")),
                default,
                default
            },
            {
                new(
                    callerUserId: new("c65c0458-b9d5-40a0-8db2-6292b8b93bf2"),
                    billingPeriodId: new("acd5446e-ca49-43f5-a8d1-e8fba22af61f")),
                [
                    new()
                    {
                        ProjectId = new("57970dca-2122-4e7e-87b1-d4a9e2b1ed91")
                    },
                    new()
                    {
                        ProjectId = new("335e9f1e-b063-4732-822a-e9a0b5c35d6d")
                    },
                    new()
                    {
                        ProjectId = new("a82e74de-2066-4f9c-b037-1d206fd7eebe")
                    }
                ],
                [
                    new(
                        entityPluralName: "gg_project_billing_periods",
                        entityData: new()
                        {
                            PeriodLookupValue = "/gg_employee_cost_periods(acd5446e-ca49-43f5-a8d1-e8fba22af61f)",
                            ProjectLookupValue = "/gg_projects(57970dca-2122-4e7e-87b1-d4a9e2b1ed91)"
                        })
                    {
                        CallerObjectId = new("c65c0458-b9d5-40a0-8db2-6292b8b93bf2")
                    },
                    new(
                        entityPluralName: "gg_project_billing_periods",
                        entityData: new()
                        {
                            PeriodLookupValue = "/gg_employee_cost_periods(acd5446e-ca49-43f5-a8d1-e8fba22af61f)",
                            ProjectLookupValue = "/gg_projects(335e9f1e-b063-4732-822a-e9a0b5c35d6d)"
                        })
                    {
                        CallerObjectId = new("c65c0458-b9d5-40a0-8db2-6292b8b93bf2")
                    },
                    new(
                        entityPluralName: "gg_project_billing_periods",
                        entityData: new()
                        {
                            PeriodLookupValue = "/gg_employee_cost_periods(acd5446e-ca49-43f5-a8d1-e8fba22af61f)",
                            ProjectLookupValue = "/gg_projects(a82e74de-2066-4f9c-b037-1d206fd7eebe)"
                        })
                    {
                        CallerObjectId = new("c65c0458-b9d5-40a0-8db2-6292b8b93bf2")
                    }
                ]
            }
        };
}