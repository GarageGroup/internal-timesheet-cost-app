using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectBilling.CalculateSet.Test;

public static partial class ProjectBillingSetCalculateHandlerTest
{
    private static readonly ProjectBillingSetCalculateIn SomeInput
        =
        new(
            callerUserId: new("59bfc33d-c2d1-406e-944a-6f5fad72bfa5"),
            billingPeriodId: new("2fea8c7d-b707-4875-a267-e7454ed6c0b5"));

    private static readonly FlatArray<DbProjectBillingPeriod> SomeDbProjectBillingPeriods
        =
        [
            new()
            {
                Id = new("2326692a-b694-445c-ae64-7b243317bc9f")
            },
            new()
            {
                Id = new("d3abfd90-d031-45b4-bfc6-8f743bda6242")
            }
        ];

    private static readonly HttpSendOut SomeHttpSuccessOutput
        =
        new()
        {
            StatusCode = HttpSuccessCode.OK
        };

    private static Mock<ISqlQueryEntitySetSupplier> BuildMockSqlApi(
        in Result<FlatArray<DbProjectBillingPeriod>, Failure<Unit>> dbProjectBillingPeriodSetResult)
    {
        var mock = new Mock<ISqlQueryEntitySetSupplier>();

        _ = mock
            .Setup(
                static a => a.QueryEntitySetOrFailureAsync<DbProjectBillingPeriod>(
                    It.IsAny<IDbQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbProjectBillingPeriodSetResult);

        return mock;
    }

    private static Mock<IHttpApi> BuildMockHttpApi(
        in Result<HttpSendOut, HttpSendFailure> result)
    {
        var mock = new Mock<IHttpApi>();

        _ = mock.Setup(static a => a.SendAsync(It.IsAny<HttpSendIn>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        return mock;
    }
}