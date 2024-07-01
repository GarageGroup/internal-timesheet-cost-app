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

    private readonly IDataverseImpersonateSupplier<IDataverseEntityCreateSupplier> dataverseApi;

    internal ProjectCostSetCreateHandler(ISqlQueryEntitySetSupplier sqlApi, IDataverseImpersonateSupplier<IDataverseEntityCreateSupplier> dataverseApi)
    {
        this.sqlApi = sqlApi;
        this.dataverseApi = dataverseApi;
    }

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

            return new(
                json: new()
                {
                    CostShare = costShare,
                    Cost = costShare * input.EmployeeCost,
                    EmployeeLookupValue = EmployeeProjectCostJson.BuildEmployeeLookupValue(input.SystemUserId),
                    PeriodLookupValue = EmployeeProjectCostJson.BuildPeriodLookupValue(input.CostPeriodId),
                    ProjectLookupValue = EmployeeProjectCostJson.BuildProjectLookupValue(timesheet.ProjectId)
                },
                callerUserId: input.CallerUserId);
        }
    }

    private sealed record class EmployeeProjectCostModel
    {
        public EmployeeProjectCostModel(
            EmployeeProjectCostJson json,
            Guid callerUserId)
        {
            Json = json;
            CallerUserId = callerUserId;
        }

        public EmployeeProjectCostJson Json { get; }

        public Guid CallerUserId { get; }
    }
}