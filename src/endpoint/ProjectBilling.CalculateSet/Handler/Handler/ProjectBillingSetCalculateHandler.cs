using System;
using System.Collections.Generic;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

using ISqlApi = ISqlQueryEntitySetSupplier;

internal sealed partial class ProjectBillingSetCalculateHandler(ISqlApi sqlApi, IHttpApi httpApi) : IProjectBillingSetCalculateHandler
{
    private const string CalculateRollupFieldUriTemplate
        =
        "/api/data/v9.2/CalculateRollupField(Target=@p1,FieldName=@p2)?@p1={{'@odata.id':'gg_project_billing_periods({0:D})'}}&@p2='{1}'";

    private const string CalculateRollupFailureBaseMessage
        =
        "An unexpected http failure occured when trying to calculate rollup field:";

    private const string CostsTotalFieldName = "gg_costs_total";

    private const string HoursTotalFieldName = "gg_hours_total";

    private static readonly PipelineParallelOption ParallelOption
        =
        new()
        {
            DegreeOfParallelism = 4
        };

    private static Result<ProjectBillingSetCalculateIn, Failure<HandlerFailureCode>> ValidateInput(ProjectBillingSetCalculateIn? input)
        =>
        input is null ? Failure.Create(HandlerFailureCode.Persistent, "Input must be not null.") : input;

    private static FlatArray<HttpSendIn> BuildHttpInputs(ProjectBillingSetCalculateIn input, FlatArray<DbProjectBillingPeriod> billingPeriods)
    {
        if (billingPeriods.IsEmpty)
        {
            return default;
        }

        FlatArray<KeyValuePair<string, string>> headers =
        [
            new("CallerObjectId", input.CallerUserId.ToString("D"))
        ];

        var builder = FlatArray<HttpSendIn>.Builder.OfLength(billingPeriods.Length * 2);

        for (var i = 0; i < billingPeriods.Length; i++)
        {
            var period = billingPeriods[i];

            builder[2 * i] = new(
                method: HttpVerb.Get,
                requestUri: string.Format(CalculateRollupFieldUriTemplate, period.Id, CostsTotalFieldName))
            {
                Headers = headers,
                SuccessType = HttpSuccessType.OnlyStatusCode
            };

            builder[2 * i + 1] = new(
                method: HttpVerb.Get,
                requestUri: string.Format(CalculateRollupFieldUriTemplate, period.Id, HoursTotalFieldName))
            {
                Headers = headers,
                SuccessType = HttpSuccessType.OnlyStatusCode
            };
        }

        return builder.MoveToFlatArray();
    }
}