using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class ProjectBillingSetCalculateHandler
{
    public ValueTask<Result<Unit, Failure<HandlerFailureCode>>> HandleAsync(
        ProjectBillingSetCalculateIn? input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            ValidateInput)
        .ForwardValue(
            InnerHandleAsync);

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> InnerHandleAsync(
        ProjectBillingSetCalculateIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => DbProjectBillingPeriod.QueryAll with
            {
                Filter = DbProjectBillingPeriod.BuildBillingPeriodFilter(@in.BillingPeriodId)
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbProjectBillingPeriod>)
        .MapFailure(
            static failure => failure.WithFailureCode(HandlerFailureCode.Transient))
        .MapSuccess(
            periods => BuildHttpInputs(input, periods))
        .ForwardParallelValue(
            CalculateRollupFieldAsync,
            ParallelOption);

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> CalculateRollupFieldAsync(
        HttpSendIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeValue(
            httpApi.SendAsync)
        .MapFailure(
            static failure => failure.ToStandardFailure(CalculateRollupFailureBaseMessage).WithFailureCode(HandlerFailureCode.Transient))
        .MapSuccess(
            Unit.From);
}