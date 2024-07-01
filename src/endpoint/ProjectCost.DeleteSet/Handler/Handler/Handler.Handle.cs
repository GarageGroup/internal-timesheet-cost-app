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
           dataverseApi.Impersonate(input.SystemUserId).GetEntitySetAsync<EmployeeProjectCostJson>)
        .Map(
            @out => new EmployeeProjectCostModel(@out, input.SystemUserId),
            static failure => failure.MapFailureCode(MapFailureCode))
        .ForwardValue(
           DeleteEmployeeProjectCostsAsync);

    private ValueTask<Result<ProjectCostSetDeleteOut, Failure<HandlerFailureCode>>> DeleteEmployeeProjectCostsAsync(
        EmployeeProjectCostModel input, CancellationToken cancellationToken)
    {
        return AsyncPipeline.Pipe(
            input.EmployeeProjectCosts, cancellationToken)
        .Pipe(
            @in => @in.Value.Map(Map))
        .PipeParallelValue(
            DeleteEmployeeProjectCostAsync)
        .MapSuccess(
            _ => new ProjectCostSetDeleteOut
            {
                HasMore = string.IsNullOrEmpty(input.EmployeeProjectCosts.NextLink) is false
            });

        DeleteEmployeeProjectCostModel Map(EmployeeProjectCostJson json)
             =>
             new(json.Id, input.SystemUserId);
    }

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> DeleteEmployeeProjectCostAsync(
        DeleteEmployeeProjectCostModel input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.EmployeeProjectCostId, cancellationToken)
        .Pipe(
            EmployeeProjectCostJson.BuildDataverseDeleteInput)
        .PipeValue(
            dataverseApi.Impersonate(input.SystemUserId).DeleteEntityAsync)
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