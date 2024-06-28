using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.CostPeriod.GetSet.Test;

public static partial class CostPeriodSetGetFuncTest
{
    private static readonly DataverseEntitySetGetOut<PeriodJson> SomePeriodJsonOut
        =
        new(
            value:
            [
                new PeriodJson()
                {
                    Id = new("ac3fad55-b805-4cad-a53e-b8c394565c9d"),
                    Name = "Some first name",
                    From = new(2024, 6, 1, 21, 22, 3),
                    To = new(2024, 6, 30, 21, 22, 3)
                },
                new PeriodJson()
                {
                    Id = new("7f17ed88-e0ca-41eb-b11f-800b0a79bc3c"),
                    Name = "Some second name",
                    From = new(2024, 5, 10, 21, 22, 3),
                    To = new(2024, 5, 15, 21, 22, 3)
                }
            ]);

    private static Mock<IDataverseEntitySetGetSupplier> BuildMockDataverseApi(
        in Result<DataverseEntitySetGetOut<PeriodJson>, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntitySetGetSupplier>();

        _ = mock
            .Setup(static a => a.GetEntitySetAsync<PeriodJson>(It.IsAny<DataverseEntitySetGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }
}