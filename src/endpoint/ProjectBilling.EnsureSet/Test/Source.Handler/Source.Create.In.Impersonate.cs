using System;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectBilling.EnsureSet.Test;

partial class ProjectBillingSetEnsureHandlerSource
{
    public static TheoryData<ProjectBillingSetEnsureIn, FlatArray<DbTimesheet>, Guid, int> InputImpersonateCreateTestData
        =>
        new()
        {
            {
                new(
                    callerUserId: new("029c361b-cca9-45eb-af46-59edf13a3bbb"),
                    billingPeriodId: new("094df0a9-23e8-435b-917d-48d04255a40b")),
                default,
                default,
                default
            },
            {
                new(
                    callerUserId: new("10698f33-0e9c-4d39-8a0d-af6bab808345"),
                    billingPeriodId: new("6f441671-5003-44ff-a3fd-24ea26459126")),
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
                new("10698f33-0e9c-4d39-8a0d-af6bab808345"),
                3
            }
        };
}