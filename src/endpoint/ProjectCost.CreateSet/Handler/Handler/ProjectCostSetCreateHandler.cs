using System;
using System.Linq;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using ISqlApi = ISqlQueryEntitySetSupplier;
using IDataverseApi = IDataverseImpersonateSupplier<IDataverseEntityCreateSupplier>;

internal sealed partial class ProjectCostSetCreateHandler(ISqlApi sqlApi, IDataverseApi dataverseApi) : IProjectCostSetCreateHandler
{
    private static readonly PipelineParallelOption ParallelOption
        =
        new()
        {
            DegreeOfParallelism = 4
        };

    private static Result<ProjectCostSetCreateIn, Failure<HandlerFailureCode>> ValidateInput(ProjectCostSetCreateIn? input)
        =>
        input is null ? Failure.Create(HandlerFailureCode.Persistent, "Input must be not null") : input;

    private static FlatArray<EmployeeProjectCostModel> BuildEmployeeProjectCostJson(
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

        EmployeeProjectCostModel MapTimesheet(DbTimesheet timesheet)
        {
            var costShare = timesheet.Duration / durationSum;

            return new()
            {
                Cost = new()
                {
                    EmployeeLookupValue = EmployeeProjectCostJson.BuildEmployeeLookupValue(input.SystemUserId),
                    PeriodLookupValue = EmployeeProjectCostJson.BuildPeriodLookupValue(input.CostPeriodId),
                    ProjectLookupValue = EmployeeProjectCostJson.BuildProjectLookupValue(timesheet.ProjectId),
                    CostShare = costShare,
                    Cost = costShare * input.EmployeeCost,
                    HoursTotal = timesheet.Duration
                },
                CallerUserId = input.CallerUserId
            };
        }
    }

    private sealed record class EmployeeProjectCostModel
    {
        public required EmployeeProjectCostJson Cost { get; init; }

        public required Guid CallerUserId { get; init; }
    }
}