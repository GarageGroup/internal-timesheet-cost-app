using DeepEqual.Syntax;
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
                RegardingObjectTypeCode = 112,
                Duration = 8
            },
            new()
            {
                ProjectId = new("6cd8c5b8-9628-493d-b790-bc010ed26367"),
                RegardingObjectTypeCode = 4,
                Duration = 4
            }
        ];

    private static readonly ProjectCostSetCreateIn SomeInput
        =
        new(
            costPeriodId: new("331d2fc0-b4b5-476b-8bd1-976cdb2caf71"),
            systemUserId: new("f887b3d0-4cc1-4033-93bb-b4728374de85"),
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

    private static Mock<IDataverseEntityCreateSupplier> BuildMockDataverseApi(
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

    private static bool AreEqual(
        DataverseEntityCreateIn<EmployeeProjectCostJson> expected,
        DataverseEntityCreateIn<EmployeeProjectCostJson> actual)
    {
        if (expected.EntityData.Cost != actual.EntityData.Cost)
        {
            return false;
        }

        expected.ShouldDeepEqual(actual);
        return true;
    }
}