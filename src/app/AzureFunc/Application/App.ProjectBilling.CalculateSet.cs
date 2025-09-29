using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [ActivityFunction(IProjectBillingSetCalculateHandler.FunctionName)]
    internal static Dependency<IProjectBillingSetCalculateHandler> UseProjectBillingSetCalculateHandler()
        =>
        Pipeline.Pipe(
            UseSqlApi())
        .With(
            UseDataverseHttpApi())
        .UseProjectBillingSetCalculateHandler();
}