using System;
using System.Net.Mime;
using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectBilling.CalculateSet.Test;

partial class ProjectBillingSetCalculateHandlerSource
{
    public static TheoryData<HttpSendFailure, Failure<HandlerFailureCode>> FailureRollupCalculateTestData
        =>
        new()
        {
            {
                default,
                new(
                    HandlerFailureCode.Transient,
                    "An unexpected http failure occured when trying to calculate rollup field: 0.")
            },
            {
                new()
                {
                    StatusCode = HttpFailureCode.NotFound,
                    Body = new()
                    {
                        Type = new(MediaTypeNames.Application.Json),
                        Content = BinaryData.FromString("Some failure message")
                    }
                },
                new(
                    HandlerFailureCode.Transient,
                    "An unexpected http failure occured when trying to calculate rollup field: 404.\nSome failure message")
            },
            {
                new()
                {
                    StatusCode = HttpFailureCode.InternalServerError,
                    ReasonPhrase = "Some reason",
                    Headers =
                    [
                        new("SomeHeader", "Some value")
                    ],
                    Body = new()
                    {
                        Content = BinaryData.FromString("Some error text.")
                    }
                },
                new(
                    HandlerFailureCode.Transient,
                    "An unexpected http failure occured when trying to calculate rollup field: 500 Some reason.\nSome error text.")
            }
        };
}