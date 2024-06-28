using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

partial class CostPeriodSetGetFunc
{
    public ValueTask<Result<CostPeriodSetGetOut, Failure<Unit>>> InvokeAsync(
        Unit input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            _ => PeriodJson.BuildDataverseSetGetInput())
        .PipeValue(
            dataverseApi.GetEntitySetAsync<PeriodJson>)
        .Map(
            static @out => new CostPeriodSetGetOut()
            {
                Periods = @out.Value.Map(MapCostPeriod)
            },
            static failure => failure.WithFailureCode(default(Unit)));

    private static CostPeriod MapCostPeriod(PeriodJson period)
        =>
        new(
            id: period.Id,
            name: period.Name,
            from: DateOnly.FromDateTime(period.From.ToLocalTime()),
            to: DateOnly.FromDateTime(period.To.ToLocalTime()));
}