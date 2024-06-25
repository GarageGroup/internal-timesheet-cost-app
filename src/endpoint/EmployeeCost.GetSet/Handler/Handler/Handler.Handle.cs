using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class EmployeeCostSetGetHandler
{
    public ValueTask<Result<EmployeeCostSetGetOut, Failure<HandlerFailureCode>>> HandleAsync(
        EmployeeCostSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => DbEmployeeCost.QueryAll with
            {
                Filter = DbEmployeeCost.BuildDefaultFilter(@in.CostPeriodId)
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbEmployeeCost>)
        .Map(
            @out => new EmployeeCostSetGetOut
            {
                EmployeeCostItems = @out.Map(MapEmployeeCost)
            },
            static failure => failure.WithFailureCode(HandlerFailureCode.Transient));

    private static EmployeeCostItem MapEmployeeCost(DbEmployeeCost dbEmployeeCost)
        =>
        new(
            systemUserId: dbEmployeeCost.UserId,
            employeeCost: dbEmployeeCost.Cost);
}