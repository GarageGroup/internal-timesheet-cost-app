using System;
using System.Linq;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class ProjectCostSetCreateHandler : IProjectCostSetCreateHandler
{
    private static readonly PipelineParallelOption ParallelOption
        =
        new()
        {
            DegreeOfParallelism = 4
        };

    private readonly ISqlQueryEntitySetSupplier sqlApi;

    private readonly IDataverseEntityCreateSupplier dataverseApi;

    internal ProjectCostSetCreateHandler(ISqlQueryEntitySetSupplier sqlApi, IDataverseEntityCreateSupplier dataverseApi)
    {
        this.sqlApi = sqlApi;
        this.dataverseApi = dataverseApi;
    }

    private static Result<ProjectCostSetCreateIn, Failure<HandlerFailureCode>> ValidateInput(ProjectCostSetCreateIn? input)
        =>
        input is null ? Failure.Create(HandlerFailureCode.Persistent, "Input must be not null") : input;

    private static FlatArray<EmployeeProjectCostJson> BuildEmployeeProjectCostJson(
        ProjectCostSetCreateIn input, FlatArray<DbTimesheet> timesheets)
    {
        if (timesheets.IsEmpty)
        {
            return default;
        }

        var durationSum = timesheets.AsEnumerable().Sum(GetDuration);
        return timesheets.Map(MapTimesheet);

        static decimal GetDuration(DbTimesheet timesheet)
            =>
            timesheet.Duration;

        EmployeeProjectCostJson MapTimesheet(DbTimesheet timesheet)
            =>
            new()
            {
                Cost = timesheet.Duration * input.EmployeeCost / durationSum,
                EmployeeLookupValue = EmployeeProjectCostJson.BuildEmployeeLookupValue(input.SystemUserId),
                PeriodLookupValue = EmployeeProjectCostJson.BuildPeriodLookupValue(input.CostPeriodId),
                ExtensionData = EmployeeProjectCostJson.BuildExtensionData(timesheet.ProjectId, timesheet.RegardingObjectTypeCode)
            };
    }
}