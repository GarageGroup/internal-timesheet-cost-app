using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.CostPeriod.GetSet.Test;

internal static partial class CostPeriodSetGetFuncSource
{
    public static TheoryData<DataverseEntitySetGetOut<PeriodJson>, CostPeriodSetGetOut> OutputTestData
        =>
        new()
        {
            {
                new(
                    value:
                    [
                        new PeriodJson()
                        {
                            Id = new("48c45953-b49d-4b78-bf3a-3567a96abb07"),
                            Name = "Some first name",
                            From = new(2024, 6, 1, 12, 23, 1),
                            To = new(2024, 6, 30, 12, 22, 3)
                        },
                        new PeriodJson()
                        {
                            Id = new("505e8431-c90c-4069-9c9d-18413b4e54e0"),
                            Name = "Some second name",
                            From = new(2024, 5, 10, 12, 22, 3),
                            To = new(2024, 5, 15, 12, 22, 3)
                        }
                    ]),
                new()
                {
                    Periods =
                    [
                        new Timesheet.CostPeriod(
                            id: new("48c45953-b49d-4b78-bf3a-3567a96abb07"),
                            name: "Some first name",
                            from: new(2024, 6, 1),
                            to: new(2024, 6, 30)),
                        new Timesheet.CostPeriod(
                            id: new("505e8431-c90c-4069-9c9d-18413b4e54e0"),
                            name: "Some second name",
                            from: new(2024, 5, 10),
                            to: new(2024, 5, 15))
                    ]
                }
            }
        };
}