using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class CreatingCostSetStartHandler
{
    public ValueTask<Result<IOrchestrationInstanceId, Failure<HandlerFailureCode>>> HandleAsync(
        CreatingCostSetStartIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new OrchestrationInstanceScheduleIn<CreatingCostSetOrchestrateIn>(
                orchestratorName: ICreatingCostSetOrchestrateHandler.FunctionName,
                value: new(@in.SystemUserId, @in.CostPeriodId)))
        .PipeValue(
            orchestrationInstanceApi.ScheduleInstanceAsync)
        .MapSuccess(
            static @out => @out.InstanceId);
}