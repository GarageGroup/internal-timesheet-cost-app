using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using GarageGroup.TestConsoleApp;

namespace GarageGroup.Internal.Timesheet;

partial class ProjectCostSetCreateHandler
{
    public ValueTask<Result<Unit, Failure<HandlerFailureCode>>> HandleAsync(
        ProjectCostSetCreateIn? input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            ValidateInput)
        .ForwardValue(
            InnerHandleAsync);

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> InnerHandleAsync(
        ProjectCostSetCreateIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => DbTimesheet.QueryAll with
            {
                Filter = new DbCombinedFilter(DbLogicalOperator.And)
                {
                    Filters =
                    [
                        DbTimesheet.BuildOwnerFilter(@in.SystemUserId),
                        DbTimesheet.BuildDateFilter(@in.CostPeriodId),
                    ]
                },
                GroupByFields = DbTimesheet.DefaultGroups
            })
        .PipeValue(
            sqlApi.QueryEntitySetOrFailureAsync<DbTimesheet>)
        .Map(
            timesheets => BuildEmployeeProjectCostJson(input, timesheets),
            static failure => failure.WithFailureCode(HandlerFailureCode.Transient))
        .ForwardParallelValue(
            CreateProjectCostAsync,
            ParallelOption)
        .MapSuccess(
            Unit.From);

    private static Result<ProjectCostSetCreateIn, Failure<HandlerFailureCode>> ValidateInput(ProjectCostSetCreateIn? input)
        =>
        input is null ? Failure.Create(HandlerFailureCode.Persistent, "Input is null") : input;

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

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> CreateProjectCostAsync(
        EmployeeProjectCostJson input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            EmployeeProjectCostJson.BuildDataverseCreateInput)
        .PipeValue(
            dataverseApi.CreateEntityAsync)
        .MapFailure(
            static failure => failure.WithFailureCode(HandlerFailureCode.Transient));
}