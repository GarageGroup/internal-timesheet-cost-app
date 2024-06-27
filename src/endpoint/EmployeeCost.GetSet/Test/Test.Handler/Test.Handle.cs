using GarageGroup.Infra;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.EmployeeCost.GetSet.Test;

partial class EmployeeCostSetGetHandlerTest
{
    [Fact]
    public static async Task HandleAsync_ExpectSqlApiCalledOnce()
    {
        var mockSqlApi = BuildMockSqlApi(SomeDbEmployeeCostSet);
        var handler = new EmployeeCostSetGetHandler(mockSqlApi.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        var input = new EmployeeCostSetGetIn(new("9a91a366-735b-490a-aa6f-af8ad6194724"));

        _ = await handler.HandleAsync(input, cancellationToken);

        var expectedQuery = new DbSelectQuery("gg_employee_cost", "c")
        {
            SelectedFields = new(
                "c.gg_cost AS Cost",
                "u.systemuserid AS UserId"),
            JoinedTables =
            [
                new(DbJoinType.Inner, "systemuser", "u", "c.gg_employee_id = u.systemuserid")
            ],
            Filter = new DbParameterFilter(
                "c.gg_period_id", DbFilterOperator.Equal, Guid.Parse("9a91a366-735b-490a-aa6f-af8ad6194724"), "periodId")
        };

        mockSqlApi.Verify(f => f.QueryEntitySetOrFailureAsync<DbEmployeeCost>(expectedQuery, cancellationToken), Times.Once);
    }

    [Fact]
    public static async Task HandleAsync_DbResultIsFailure_ExpectFailure()
    {
        var sourceException = new Exception("Some error message");
        var dbFailure = sourceException.ToFailure("Some failure text");

        var mockSqlApi = BuildMockSqlApi(dbFailure);
        var handler = new EmployeeCostSetGetHandler(mockSqlApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Failure.Create(HandlerFailureCode.Transient, "Some failure text", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task HandleAsync_DbResultIsSuccess_ExpectSuccess()
    {
        FlatArray<DbEmployeeCost> dbOutput =
        [
            new()
            {
                Cost = 5903,
                UserId = new("6deb4bf3-b689-4436-a7c1-e9996b87428e")
            },
            new()
            {
                Cost = 2500.75m,
                UserId = new("6ba73643-debb-434c-8f77-dd1b9ad1f450")
            }
        ];

        var mockSqlApi = BuildMockSqlApi(dbOutput);
        var handler = new EmployeeCostSetGetHandler(mockSqlApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);

        var expected = new EmployeeCostSetGetOut
        {
            EmployeeCostItems =
            [
                new(
                    systemUserId: new("6deb4bf3-b689-4436-a7c1-e9996b87428e"),
                    employeeCost: 5903),
                new(
                    systemUserId: new("6ba73643-debb-434c-8f77-dd1b9ad1f450"),
                    employeeCost: 2500.75m),
            ]
        };

        Assert.StrictEqual(expected, actual);
    }
}