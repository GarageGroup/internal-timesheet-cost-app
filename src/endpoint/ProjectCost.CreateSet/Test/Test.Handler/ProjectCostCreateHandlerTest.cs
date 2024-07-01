using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.CreateSet.Test;

public static partial class ProjectCostCreateHandlerTest
{
    private static readonly FlatArray<DbTimesheet> SomeDbTimesheetSet
        =
        [
            new()
            {
                ProjectId = new("d1f6db66-e731-423f-a85f-0da3675e7b91"),
                Duration = 8
            },
            new()
            {
                ProjectId = new("6cd8c5b8-9628-493d-b790-bc010ed26367"),
                Duration = 4
            }
        ];

    private static readonly ProjectCostSetCreateIn SomeInput
        =
        new(
            costPeriodId: new("331d2fc0-b4b5-476b-8bd1-976cdb2caf71"),
            systemUserId: new("f887b3d0-4cc1-4033-93bb-b4728374de85"),
            callerUserId: new("5b25be13-5120-4807-979a-c4f879d547b3"),
            employeeCost: 12333);

    private static Mock<ISqlQueryEntitySetSupplier> BuildMockSqlApi(
        in Result<FlatArray<DbTimesheet>, Failure<Unit>> result)
    {
        var mock = new Mock<ISqlQueryEntitySetSupplier>();

        _ = mock
            .Setup(
                static a => a.QueryEntitySetOrFailureAsync<DbTimesheet>(
                    It.IsAny<IDbQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }

    private static Mock<IDataverseEntityCreateSupplier> BuildMockDataverseCreateApi(
        in Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntityCreateSupplier>();

        _ = mock
            .Setup(
                static a => a.CreateEntityAsync(
                    It.IsAny<DataverseEntityCreateIn<EmployeeProjectCostJson>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }

    private static Mock<IDataverseImpersonateSupplier<IDataverseEntityCreateSupplier>> BuildMockDataverseApi(
        IDataverseEntityCreateSupplier dataverseCreateSupplier)
    {
        var mock = new Mock<IDataverseImpersonateSupplier<IDataverseEntityCreateSupplier>>();

        _ = mock.Setup(static a => a.Impersonate(It.IsAny<Guid>())).Returns(dataverseCreateSupplier);

        return mock;
    }
}