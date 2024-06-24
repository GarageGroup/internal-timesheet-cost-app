using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [HttpFunction("HealthCheck", HttpMethodName.Get, Route = "health", AuthLevel = HttpAuthorizationLevel.Function)]
    internal static Dependency<IHealthCheckHandler> UseHealthCheckHandler()
        =>
        HealthCheck.UseServices(
            UseDataverseApi().UseServiceHealthCheckApi("DataverseApi"),
            UseSqlApi().UseServiceHealthCheckApi("DataverseDb"))
        .UseHealthCheckHandler();
}