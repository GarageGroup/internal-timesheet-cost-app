using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [ActivityFunction(IEmployeeCostSetGetHandler.FunctionName)]
    internal static Dependency<IEmployeeCostSetGetHandler> UseEmployeeCostSetGetHandler()
        =>
        UseSqlApi().UseEmployeeCostSetGetHandler();
}