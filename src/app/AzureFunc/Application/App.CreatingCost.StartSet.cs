using GarageGroup.Infra;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [HttpFunction(ICreatingCostSetStartHandler.FunctionName, "POST", AuthLevel = HttpAuthorizationLevel.Function, Route = "employeeCosts")]
    internal static Dependency<ICreatingCostSetStartHandler> UseCreatingCostSetStartHandler(
        [DurableClient] DurableTaskClient client)
        =>
        Dependency.Of(client).UseOrchestrationInstanceApi().UseCreatingCostSetStartHandler();
}