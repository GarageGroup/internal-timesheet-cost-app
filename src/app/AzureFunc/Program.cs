using System.Threading.Tasks;
using GarageGroup.Infra;
using Microsoft.Extensions.Hosting;

namespace GarageGroup.Internal.Timesheet;

static class Program
{
    static Task Main()
        =>
        FunctionHost.CreateFunctionsWorkerBuilderStandard().Build().RunAsync();
}