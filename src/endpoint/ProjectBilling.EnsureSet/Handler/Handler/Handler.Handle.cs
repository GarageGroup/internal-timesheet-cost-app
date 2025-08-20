using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class ProjectBillingSetEnsureHandler
{
    public ValueTask<Result<Unit, Failure<HandlerFailureCode>>> HandleAsync(
        ProjectBillingSetEnsureIn? input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            ValidateInput)
        .ForwardValue(
            InnerHandleAsync);

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> InnerHandleAsync(
        ProjectBillingSetEnsureIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeValue(
            GetTimesheetsAsync)
        .MapSuccess(
            timesheets => BuildProjectBillingPeriods(input, timesheets))
        .ForwardParallelValue(
            CreateProjectBillingPeriodAsync,
            ParallelOption)
        .MapSuccess(
            Unit.From);

    private ValueTask<Result<FlatArray<DbTimesheet>, Failure<HandlerFailureCode>>> GetTimesheetsAsync(
        ProjectBillingSetEnsureIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => DbTimesheet.QueryAll with
            {
                Filter = new DbCombinedFilter(DbLogicalOperator.And)
                {
                    Filters =
                    [
                        DbTimesheet.ProjectNotNullFiler,
                        DbTimesheet.BuildDateFilter(@in.BillingPeriodId),
                        DbTimesheet.BuildBillingPeriodFilter(@in.BillingPeriodId)
                    ]
                }
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbTimesheet>)
        .MapFailure(
            static failure => failure.WithFailureCode(HandlerFailureCode.Transient));

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> CreateProjectBillingPeriodAsync(
        ProjectBillingPeriodModel input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.ProjectBillingPeriod, cancellationToken)
        .Pipe(
            ProjectBillingPeriodJson.BuildDataverseCreateInput)
        .PipeValue(
            dataverseApi.Impersonate(input.CallerUserId).CreateEntityAsync)
        .Recover(
            static failure => failure.FailureCode switch
            {
                DataverseFailureCode.DuplicateRecord => Result.Success<Unit>(default).With<Failure<HandlerFailureCode>>(),
                _ => failure.MapFailureCode(MapFailureCode)
            });

    private static HandlerFailureCode MapFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.UserNotEnabled => HandlerFailureCode.Persistent,
            DataverseFailureCode.PrivilegeDenied => HandlerFailureCode.Persistent,
            _ => HandlerFailureCode.Transient
        };
}