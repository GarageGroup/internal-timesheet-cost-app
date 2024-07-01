using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.CreateSet.Test;

internal static partial class ProjectCostCreateHandlerSource
{
    public static TheoryData<ProjectCostSetCreateIn, FlatArray<DbTimesheet>, Guid, int> InputImpersonateCreateTestData
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
                new("5b25be13-5120-4807-979a-c4f879d547b3"),
                4
            }
        };
}