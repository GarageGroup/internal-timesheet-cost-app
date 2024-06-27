using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.EmployeeCost.GetSet.Test;

public static partial class EmployeeCostSetGetHandlerTest
{
    private static readonly FlatArray<DbEmployeeCost> SomeDbEmployeeCostSet
        =
        [
            new()
            {
                Cost = 123,
                UserId = new("6ece3952-842a-4e28-abd9-429f942a98df")
            },
            new()
            {
                Cost = 321,
                UserId = new("cf0962b9-95ce-48cd-b83e-f78737ba8196")
            }
        ];

    private static readonly EmployeeCostSetGetIn SomeInput
        =
        new(
            costPeriodId: new("7ba3170c-0777-4755-8e99-fb4e3ff1d240"));

    private static Mock<ISqlQueryEntitySetSupplier> BuildMockSqlApi(
        in Result<FlatArray<DbEmployeeCost>, Failure<Unit>> result)
    {
        var mock = new Mock<ISqlQueryEntitySetSupplier>();

        _ = mock
            .Setup(
                static a => a.QueryEntitySetOrFailureAsync<DbEmployeeCost>(
                    It.IsAny<IDbQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock;
    }
}