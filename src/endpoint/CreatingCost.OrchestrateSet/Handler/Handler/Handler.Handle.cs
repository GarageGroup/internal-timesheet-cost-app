using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class CreatingCostSetOrchestrateHandler
{
    public ValueTask<Result<CreatingCostSetOrchestrateOut, Failure<HandlerFailureCode>>> HandleAsync(
        CreatingCostSetOrchestrateIn input, CancellationToken cancellationToken)
        =>
        OrchestrationAsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeParallel(
            DeleteProjectCostsAsync,
            GetEmployeeCostsAsync)
        .ForwardParallel(
            CreateProjectCostsAsync)
        .MapSuccess(
            static costs => new CreatingCostSetOrchestrateOut
            {
                EmployeeCosts = costs
            });

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
            @out => MapEmployeeCosts(input.CostPeriodId, @out.Value.EmployeeCostItems));

    private Task<Result<CreatingCostItem, Failure<HandlerFailureCode>>> CreateProjectCostsAsync(
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
                    employeeCost: @in.Cost)))
        .PipeValue(
            orchestrationActivityApi.CallActivityAsync)
        .MapSuccess(
            _ => new CreatingCostItem(input.EmployeeName, true));

    private async Task<Result<Unit, Failure<HandlerFailureCode>>> DeleteProjectCostsAsync(
        CreatingCostSetOrchestrateIn input, CancellationToken cancellationToken)
    {
        while (true)
        {
            var @in = new OrchestrationActivityCallIn<ProjectCostSetDeleteIn>(
                activityName: IProjectCostSetDeleteHandler.FunctionName,
                value: new(input.CostPeriodId, MaxCostItems));

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