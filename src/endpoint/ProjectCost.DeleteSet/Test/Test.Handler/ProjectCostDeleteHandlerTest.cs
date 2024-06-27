using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.DeleteSet.Test;

public static partial class ProjectCostDeleteHandlerTest
{
    private static readonly DataverseEntitySetGetOut<EmployeeProjectCostJson> SomeEmployeeProjectCostJsonOut
        =
        new(
            value:
            [
                new()
                {
                    Id = new("97335476-1d4a-4206-a1e2-5011c3a864a3")
                },
                new()
                {
                    Id = new("081c29c1-ae4b-441a-90b0-e36550094980")
                }
            ]);

    private static readonly ProjectCostSetDeleteIn SomeInput
        =
        new(
            costPeriodId: new("e178133b-8df4-4efb-be1a-2d1cc77e6802"),
            maxItems: 32);

    private static Mock<IDataverseApiClient> BuildMockDataverseApi<TOut>(
        in Result<DataverseEntitySetGetOut<TOut>, Failure<DataverseFailureCode>> setGetResult,
        in Result<Unit, Failure<DataverseFailureCode>> deleteResult)
    {
        var mock = new Mock<IDataverseApiClient>();

        _ = mock
            .Setup(static a => a.GetEntitySetAsync<TOut>(It.IsAny<DataverseEntitySetGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(setGetResult);

        _ = mock
            .Setup(static a => a.DeleteEntityAsync(It.IsAny<DataverseEntityDeleteIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(deleteResult);

        return mock;
    }
}