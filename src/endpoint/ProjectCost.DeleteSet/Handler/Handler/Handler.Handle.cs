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
           dataverseApi.GetEntitySetAsync<EmployeeProjectCostJson>)
        .MapFailure(
            static failure => failure.WithFailureCode(HandlerFailureCode.Transient))
        .ForwardValue(
           DeleteEmployeeProjectCostsAsync);

    private ValueTask<Result<ProjectCostSetDeleteOut, Failure<HandlerFailureCode>>> DeleteEmployeeProjectCostsAsync(
        DataverseEntitySetGetOut<EmployeeProjectCostJson> input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.Value, cancellationToken)
        .PipeParallelValue(
            DeleteEmployeeProjectCostAsync)
        .Map(
            _ => new ProjectCostSetDeleteOut
            {
                HasMore = string.IsNullOrEmpty(input.NextLink) is false
            },
            static failure => failure.WithFailureCode(HandlerFailureCode.Transient));

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> DeleteEmployeeProjectCostAsync(
        EmployeeProjectCostJson input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.Id, cancellationToken)
        .Pipe(
            EmployeeProjectCostJson.BuildDataverseDeleteInput)
        .PipeValue(
            dataverseApi.DeleteEntityAsync)
        .MapFailure(
            static failure => failure.WithFailureCode(HandlerFailureCode.Transient));
}