using GarageGroup.Infra;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [OrchestrationFunction(ICreatingCostSetOrchestrateHandler.FunctionName)]
    internal static Dependency<ICreatingCostSetOrchestrateHandler> UseCreatingCostSetOrchestrateHandler(
        [OrchestrationTrigger] TaskOrchestrationContext context)
        =>
        Dependency.Of(context).UseOrchestrationActivityApi("OrchestrationActivity").UseCreatingCostSetOrchestrateHandler();
}