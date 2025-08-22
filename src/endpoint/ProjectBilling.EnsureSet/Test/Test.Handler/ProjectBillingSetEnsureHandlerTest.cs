using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectBilling.EnsureSet.Test;

public static partial class ProjectBillingSetEnsureHandlerTest
{
    private static readonly ProjectBillingSetEnsureIn SomeInput
        =
        new(
            callerUserId: new("eabb2ead-5d29-465c-9ed7-752bdd900aac"),
            billingPeriodId: new("f30aa33a-3cef-45ea-a269-f44328e1ec96"));

    private static readonly FlatArray<DbTimesheet> SomeDbTimesheetSet
        =
        [
            new()
            {
                ProjectId = new("c5c4c394-646a-4d86-a62b-4e904601f55a")
            },
            new()
            {
                ProjectId = new("548777e5-f438-434e-bf41-2b5a56c2d147")
            },
            new()
            {
                ProjectId = new("7fa6abc1-249f-4b31-8fba-0fe1f2685f60")
            }
        ];

    private static Mock<ISqlQueryEntitySetSupplier> BuildMockSqlApi(
        in Result<FlatArray<DbTimesheet>, Failure<Unit>> dbTimesheetSetResult)
    {
        var mock = new Mock<ISqlQueryEntitySetSupplier>();

        _ = mock
            .Setup(
                static a => a.QueryEntitySetOrFailureAsync<DbTimesheet>(
                    It.IsAny<IDbQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbTimesheetSetResult);

        return mock;
    }

    private static Mock<IDataverseEntityCreateSupplier> BuildMockDataverseApi(
        in Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntityCreateSupplier>();

        _ = mock
            .Setup(
                static a => a.CreateEntityAsync(
                    It.IsAny<DataverseEntityCreateIn<ProjectBillingPeriodJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }
}