using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [ActivityFunction(IProjectBillingSetEnsureHandler.FunctionName)]
    internal static Dependency<IProjectBillingSetEnsureHandler> UseProjectBillingSetEnsureHandler()
        =>
        Pipeline.Pipe(
            UseSqlApi())
        .With(
            UseDataverseApi())
        .UseProjectBillingSetEnsureHandler();
}