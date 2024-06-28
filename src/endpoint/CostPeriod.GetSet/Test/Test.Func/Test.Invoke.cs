using Castle.Components.DictionaryAdapter.Xml;
using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.CostPeriod.GetSet.Test;

partial class CostPeriodSetGetFuncTest
{
    [Fact]
    public static async Task InvokeAsync_ExpectDataverseSetGetCalledOnce()
    {
        var mockDataverseApi = BuildMockDataverseApi(SomePeriodJsonOut);
        var func = new CostPeriodSetGetFunc(mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(false);
        _ = await func.InvokeAsync(default, cancellationToken);

        var expectedInput = new DataverseEntitySetGetIn(
            entityPluralName: "gg_employee_cost_periods",
            selectFields: ["gg_employee_cost_periodid", "gg_name", "gg_from_date", "gg_to_date"],
            filter: default,
            expandFields: default,
            orderBy:
            [
                new(
                    fieldName: "gg_to_date",
                    direction: DataverseOrderDirection.Descending)
            ]);

        mockDataverseApi.Verify(a => a.GetEntitySetAsync<PeriodJson>(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized)]
    [InlineData(DataverseFailureCode.RecordNotFound)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange)]
    [InlineData(DataverseFailureCode.UserNotEnabled)]
    [InlineData(DataverseFailureCode.PrivilegeDenied)]
    [InlineData(DataverseFailureCode.Throttling)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound)]
    [InlineData(DataverseFailureCode.DuplicateRecord)]
    [InlineData(DataverseFailureCode.InvalidPayload)]
    [InlineData(DataverseFailureCode.InvalidFileSize)]
    public static async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode)
    {
        var sourceException = new Exception("Some error message");
        var dataverseFailure = sourceException.ToFailure(sourceFailureCode, "Some failure message");

        var mockDataverseApi = BuildMockDataverseApi(dataverseFailure);
        var func = new CostPeriodSetGetFunc(mockDataverseApi.Object);

        var actual = await func.InvokeAsync(default, default);
        var expected = Failure.Create(default(Unit), "Some failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(CostPeriodSetGetFuncSource.OutputTestData), MemberType = typeof(CostPeriodSetGetFuncSource))]
    internal static async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess(
        DataverseEntitySetGetOut<PeriodJson> dataverseOut, CostPeriodSetGetOut expected)
    {
        var mockDataverseApi = BuildMockDataverseApi(dataverseOut);
        var func = new CostPeriodSetGetFunc(mockDataverseApi.Object);

        var cancellationToken = new CancellationToken(false);
        var actual = await func.InvokeAsync(default, cancellationToken);

        Assert.StrictEqual(expected, actual);
    }
}