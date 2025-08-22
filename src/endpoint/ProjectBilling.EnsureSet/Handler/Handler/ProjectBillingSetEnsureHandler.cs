using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using ISqlApi = ISqlQueryEntitySetSupplier;
using IDataverseApi = IDataverseEntityCreateSupplier;

internal sealed partial class ProjectBillingSetEnsureHandler(ISqlApi sqlApi, IDataverseApi dataverseApi) : IProjectBillingSetEnsureHandler
{
    private static readonly PipelineParallelOption ParallelOption
        =
        new()
        {
            DegreeOfParallelism = 4
        };

    private static Result<ProjectBillingSetEnsureIn, Failure<HandlerFailureCode>> ValidateInput(ProjectBillingSetEnsureIn? input)
        =>
        input is null ? Failure.Create(HandlerFailureCode.Persistent, "Input must be not null.") : input;

    private static FlatArray<ProjectBillingPeriodModel> BuildProjectBillingPeriods(
        ProjectBillingSetEnsureIn input, FlatArray<DbTimesheet> timesheets)
    {
        if (timesheets.IsEmpty)
        {
            return default;
        }

        return timesheets.Map(MapTimesheet);

        ProjectBillingPeriodModel MapTimesheet(DbTimesheet timesheet)
        {
            return new()
            {
                ProjectBillingPeriod = new()
                {
                    PeriodLookupValue = ProjectBillingPeriodJson.BuildPeriodLookupValue(input.BillingPeriodId),
                    ProjectLookupValue = ProjectBillingPeriodJson.BuildProjectLookupValue(timesheet.ProjectId)
                },
                CallerUserId = input.CallerUserId
            };
        }
    }

    private sealed record class ProjectBillingPeriodModel
    {
        public required ProjectBillingPeriodJson ProjectBillingPeriod { get; init; }

        public required Guid CallerUserId { get; init; }
    }
}