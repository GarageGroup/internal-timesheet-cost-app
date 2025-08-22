using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectBilling.CalculateSet.Test;

partial class ProjectBillingSetCalculateHandlerTest
{
    [Fact]
    public static async Task HandleAsync_InputIsNull_ExpectFailure()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbProjectBillingPeriods);
        var mockHttpApi = BuildMockHttpApi(SomeHttpSuccessOutput);

        var handler = new ProjectBillingSetCalculateHandler(mockSqlApi.Object, mockHttpApi.Object);

        var actual = await handler.HandleAsync(null, default);
        var expected = Failure.Create(HandlerFailureCode.Persistent, "Input must be not null.");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_InputIsNotNull_ExpectDbProjectBillingPeriodQueryCalledOnce()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbProjectBillingPeriods);
        var mockHttpApi = BuildMockHttpApi(SomeHttpSuccessOutput);

        var handler = new ProjectBillingSetCalculateHandler(mockSqlApi.Object, mockHttpApi.Object);

        var input = new ProjectBillingSetCalculateIn(
            callerUserId: new("55763139-f2e7-4e16-abba-39710f7f2f44"),
            billingPeriodId: new("8f9cc40a-a268-434e-a9ac-0fe1be948768"));

        _ = await handler.HandleAsync(input, default);

        var expectedQuery = new DbSelectQuery("gg_project_billing_period", "p")
        {
            SelectedFields = new("p.gg_project_billing_periodid AS Id"),
            Filter = new DbParameterFilter(
                fieldName: "p.gg_billingperiod_id",
                @operator: DbFilterOperator.Equal,
                fieldValue: Guid.Parse("8f9cc40a-a268-434e-a9ac-0fe1be948768"),
                parameterName: "billingPeriodId")
        };

        mockSqlApi.Verify(
            f => f.QueryEntitySetOrFailureAsync<DbProjectBillingPeriod>(expectedQuery, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public static async Task HandleAsync_DbProjectBillingSetQueryResultIsFailure_ExpectTransientFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some failure message.");

        var mockSqlApi = BuildMockSqlApi(dbFailure);

        var mockHttpApi = BuildMockHttpApi(SomeHttpSuccessOutput);
        var handler = new ProjectBillingSetCalculateHandler(mockSqlApi.Object, mockHttpApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Failure.Create(HandlerFailureCode.Transient, "Some failure message.", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_DbProjectBillingSetQueryResultIsEmptySuccess_ExpectSuccess()
    {
        var mockSqlApi = BuildMockSqlApi(default(FlatArray<DbProjectBillingPeriod>));
        var handler = new ProjectBillingSetCalculateHandler(mockSqlApi.Object, Mock.Of<IHttpApi>());

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_DbProjectBillingSetQueryResultIsNotEmptySuccess_ExpectHttpRollupCalculateCalledExactTimes()
    {
        FlatArray<DbProjectBillingPeriod> dbProjectBillingPeriods =
        [
            new()
            {
                Id = new("fd51d5f5-b6f5-4b49-a2b1-ec9c04aa00e9")
            },
            new()
            {
                Id = new("9637cee0-dab4-45ee-8244-38bb3e1a521e")
            }
        ];

        var mockSqlApi = BuildMockSqlApi(dbProjectBillingPeriods);

        var mockHttpApi = BuildMockHttpApi(SomeHttpSuccessOutput);
        var handler = new ProjectBillingSetCalculateHandler(mockSqlApi.Object, mockHttpApi.Object);

        var input = new ProjectBillingSetCalculateIn(
            callerUserId: new("e9a16ea1-e5e2-4100-8836-cfdbf84c2a2c"),
            billingPeriodId: new("63b1b442-e37c-4f08-aad6-e911c6b75b80"));

        var actual = await handler.HandleAsync(input, default);

        FlatArray<HttpSendIn> expectedHttpInputs =
        [
            new(HttpVerb.Get, BuildExpectRequestUri("fd51d5f5-b6f5-4b49-a2b1-ec9c04aa00e9", "gg_costs_total"))
            {
                Headers =
                [
                    new("CallerObjectId", "e9a16ea1-e5e2-4100-8836-cfdbf84c2a2c")
                ],
                SuccessType = HttpSuccessType.OnlyStatusCode
            },
            new(HttpVerb.Get, BuildExpectRequestUri("fd51d5f5-b6f5-4b49-a2b1-ec9c04aa00e9", "gg_hours_total"))
            {
                Headers =
                [
                    new("CallerObjectId", "e9a16ea1-e5e2-4100-8836-cfdbf84c2a2c")
                ],
                SuccessType = HttpSuccessType.OnlyStatusCode
            },
            new(HttpVerb.Get, BuildExpectRequestUri("9637cee0-dab4-45ee-8244-38bb3e1a521e", "gg_costs_total"))
            {
                Headers =
                [
                    new("CallerObjectId", "e9a16ea1-e5e2-4100-8836-cfdbf84c2a2c")
                ],
                SuccessType = HttpSuccessType.OnlyStatusCode
            },
            new(HttpVerb.Get, BuildExpectRequestUri("9637cee0-dab4-45ee-8244-38bb3e1a521e", "gg_hours_total"))
            {
                Headers =
                [
                    new("CallerObjectId", "e9a16ea1-e5e2-4100-8836-cfdbf84c2a2c")
                ],
                SuccessType = HttpSuccessType.OnlyStatusCode
            }
        ];

        foreach (var expectedHttpInput in expectedHttpInputs)
        {
            mockHttpApi.Verify(f => f.SendAsync(expectedHttpInput, It.IsAny<CancellationToken>()), Times.Once);
        }

        static string BuildExpectRequestUri(string id, string fieldName)
            =>
            "/api/data/v9.2/CalculateRollupField(Target=@p1,FieldName=@p2)"
            + "?@p1={'@odata.id':'gg_project_billing_periods(" + id + ")'}"
            + "&@p2='" + fieldName + "'";
    }

    [Theory]
    [MemberData(
        nameof(ProjectBillingSetCalculateHandlerSource.FailureRollupCalculateTestData),
        MemberType = typeof(ProjectBillingSetCalculateHandlerSource))]
    public static async Task HandleAsync_HttpRollupCalculateResultIsFailure_ExpectFailure(
        HttpSendFailure httpFailure, Failure<HandlerFailureCode> expected)
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbProjectBillingPeriods);
        var mockHttpApi = BuildMockHttpApi(httpFailure);

        var handler = new ProjectBillingSetCalculateHandler(mockSqlApi.Object, mockHttpApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [InlineData(HttpSuccessCode.OK)]
    [InlineData(HttpSuccessCode.NoContent)]
    public static async Task HandleAsync_HttpRollupCalculateResultIsSuccess_ExpectSuccess(
        HttpSuccessCode httpSuccessCode)
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbProjectBillingPeriods);

        var httpOutput = new HttpSendOut
        {
            StatusCode = httpSuccessCode
        };
        var mockHttpApi = BuildMockHttpApi(httpOutput);

        var handler = new ProjectBillingSetCalculateHandler(mockSqlApi.Object, mockHttpApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}