using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [ActivityFunction(IProjectCostSetDeleteHandler.FunctionName)]
    internal static Dependency<IProjectCostSetDeleteHandler> UseProjectCostSetDeleteHandler()
        =>
        UseDataverseApi().UseProjectCostSetDeleteHandler();
}