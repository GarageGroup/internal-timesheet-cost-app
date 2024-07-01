using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class ProjectCostSetCreateHandler
{
    public ValueTask<Result<Unit, Failure<HandlerFailureCode>>> HandleAsync(
        ProjectCostSetCreateIn? input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            ValidateInput)
        .ForwardValue(
            InnerHandleAsync);

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> InnerHandleAsync(
        ProjectCostSetCreateIn input, CancellationToken cancellationToken)
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
                        DbTimesheet.BuildOwnerFilter(@in.SystemUserId),
                        DbTimesheet.BuildDateFilter(@in.CostPeriodId),
                    ]
                }
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbTimesheet>)
        .Map(
            timesheets => BuildEmployeeProjectCostJson(input, timesheets),
            static failure => failure.WithFailureCode(HandlerFailureCode.Transient))
        .ForwardParallelValue(
            CreateProjectCostAsync,
            ParallelOption)
        .MapSuccess(
            Unit.From);

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> CreateProjectCostAsync(
        EmployeeProjectCostModel input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.Json, cancellationToken)
        .Pipe(
            EmployeeProjectCostJson.BuildDataverseCreateInput)
        .PipeValue(
            dataverseApi.Impersonate(input.CallerUserId).CreateEntityAsync)
        .MapFailure(
            static failure => failure.MapFailureCode(MapFailureCode));

    private static HandlerFailureCode MapFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.UserNotEnabled => HandlerFailureCode.Persistent,
            DataverseFailureCode.PrivilegeDenied => HandlerFailureCode.Persistent,
            _ => HandlerFailureCode.Transient
        };
}