using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class ProjectCostSetDeleteHandler
{
    public ValueTask<Result<ProjectCostSetDeleteOut, Failure<HandlerFailureCode>>> HandleAsync(
        ProjectCostSetDeleteIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
           input, cancellationToken)
        .Pipe(
           static @in => EmployeeProjectCostJson.BuildDataverseSetGetInput(@in.CostPeriodId, @in.MaxItems))
        .PipeValue(
           dataverseApi.Impersonate(input.CallerUserId).GetEntitySetAsync<EmployeeProjectCostJson>)
        .Map(
            costs => new EmployeeProjectCostModel
            {
                EmployeeProjectCosts = costs,
                CallerUserId = input.CallerUserId
            },
            static failure => failure.MapFailureCode(MapFailureCode))
        .ForwardValue(
           DeleteEmployeeProjectCostsAsync);

    private ValueTask<Result<ProjectCostSetDeleteOut, Failure<HandlerFailureCode>>> DeleteEmployeeProjectCostsAsync(
        EmployeeProjectCostModel input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            GetEmployeeProjectCosts)
        .PipeParallelValue(
            DeleteEmployeeProjectCostAsync)
        .MapSuccess(
            _ => new ProjectCostSetDeleteOut
            {
                HasMore = string.IsNullOrEmpty(input.EmployeeProjectCosts.NextLink) is false
            });

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> DeleteEmployeeProjectCostAsync(
        DeleteEmployeeProjectCostModel input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.EmployeeProjectCostId, cancellationToken)
        .Pipe(
            EmployeeProjectCostJson.BuildDataverseDeleteInput)
        .PipeValue(
            dataverseApi.Impersonate(input.CallerUserId).DeleteEntityAsync)
        .MapFailure(
            static failure => failure.MapFailureCode(MapFailureCode));
}