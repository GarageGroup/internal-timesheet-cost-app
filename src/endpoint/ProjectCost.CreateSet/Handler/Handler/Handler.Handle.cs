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
        .PipeParallelValue(
            GetTimesheetsAsync,
            GetTotalCostAsync)
        .MapSuccess(
            @out => BuildEmployeeProjectCostJson(input, @out.Item2, @out.Item1))
        .ForwardParallelValue(
            CreateProjectCostAsync,
            ParallelOption)
        .MapSuccess(
            Unit.From);

    private ValueTask<Result<FlatArray<DbTimesheet>, Failure<HandlerFailureCode>>> GetTimesheetsAsync(
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
                        DbTimesheet.BuildDateFilter(@in.CostPeriodId)
                    ]
                }
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbTimesheet>)
        .MapFailure(
            static failure => failure.WithFailureCode(HandlerFailureCode.Transient));

    private ValueTask<Result<DbProjectCost, Failure<HandlerFailureCode>>> GetTotalCostAsync(
        ProjectCostSetCreateIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => DbProjectCost.QueryAll with
            {
                Filter = new DbCombinedFilter(DbLogicalOperator.And)
                {
                    Filters =
                    [
                        DbProjectCost.ManualCreationFilter,
                        DbProjectCost.BuildEmployeeIdFilter(@in.SystemUserId),
                        DbProjectCost.BuildPeriodIdFilter(@in.CostPeriodId)
                    ]
                }
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbProjectCost>)
        .Map(
            static dbCosts => dbCosts.IsEmpty ? new() : dbCosts[0],
            static failure => failure.WithFailureCode(HandlerFailureCode.Transient));

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> CreateProjectCostAsync(
        EmployeeProjectCostModel input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.Cost, cancellationToken)
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