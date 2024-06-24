using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

partial class Application
{
    [ActivityFunction(IProjectCostSetCreateHandler.FunctionName)]
    internal static Dependency<IProjectCostSetCreateHandler> UseProjectCostSetCreateHandler()
        =>
        Pipeline.Pipe(
            UseSqlApi())
        .With(
            UseDataverseApi())
        .UseProjectCostSetCreateHandler();
}