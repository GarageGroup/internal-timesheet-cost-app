﻿using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class CreatingCostSetOrchestrateHandler
{
    public ValueTask<Result<Unit, Failure<HandlerFailureCode>>> HandleAsync(
        CreatingCostSetOrchestrateIn input, CancellationToken cancellationToken)
        =>
        OrchestrationAsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeParallel(
            DeleteProjectCostsAsync,
            GetEmployeeCostsAsync)
        .ForwardParallel(
            CreateProjectCostsAsync);

    private Task<Result<FlatArray<EmployeeCost>, Failure<HandlerFailureCode>>> GetEmployeeCostsAsync(
        CreatingCostSetOrchestrateIn input, CancellationToken cancellationToken)
        =>
        OrchestrationAsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new OrchestrationActivityCallIn<EmployeeCostSetGetIn>(
                activityName: IEmployeeCostSetGetHandler.FunctionName,
                value: new(@in.CostPeriodId)))
        .PipeValue(
            orchestrationActivityApi.CallActivityAsync<EmployeeCostSetGetIn, EmployeeCostSetGetOut>)
        .MapSuccess(
            @out => MapEmployeeCosts(input, @out.Value.EmployeeCostItems));

    private Task<Result<Unit, Failure<HandlerFailureCode>>> CreateProjectCostsAsync(
        EmployeeCost input, CancellationToken cancellationToken)
        =>
        OrchestrationAsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new OrchestrationActivityCallIn<ProjectCostSetCreateIn>(
                activityName: IProjectCostSetCreateHandler.FunctionName,
                value: new(
                    costPeriodId: @in.CostPeriodId,
                    systemUserId: @in.SystemUserId,
                    callerUserId: @in.CallerUserId,
                    employeeCost: @in.Cost)))
        .PipeValue(
            orchestrationActivityApi.CallActivityAsync);

    private async Task<Result<Unit, Failure<HandlerFailureCode>>> DeleteProjectCostsAsync(
        CreatingCostSetOrchestrateIn input, CancellationToken cancellationToken)
    {
        while (true)
        {
            var @in = new OrchestrationActivityCallIn<ProjectCostSetDeleteIn>(
                activityName: IProjectCostSetDeleteHandler.FunctionName,
                value: new(input.CallerUserId, input.CostPeriodId, MaxCostItems));

            var result = await orchestrationActivityApi.CallActivityAsync<ProjectCostSetDeleteIn, ProjectCostSetDeleteOut>(@in, cancellationToken);
            if (result.IsFailure)
            {
                return result.FailureOrThrow();
            }

            if (result.SuccessOrThrow().Value.HasMore is false)
            {
                return Result.Success<Unit>(default);
            }
        };
    }
}