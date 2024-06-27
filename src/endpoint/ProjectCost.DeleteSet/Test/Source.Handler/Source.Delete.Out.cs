using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.ProjectCost.DeleteSet.Test;

partial class ProjectCostDeleteHandlerSource
{
    public static TheoryData<DataverseEntitySetGetOut<EmployeeProjectCostJson>, ProjectCostSetDeleteOut> OutputDeleteTestData
        =>
        new()
        {
            {
                default,
                default
            },
            {
                new(
                    value:
                    [
                        new()
                        {
                            Id = new("02e072d5-127e-447b-92b8-bbfa183b7392")
                        },
                        new()
                        {
                            Id = new("02e072d5-127e-447b-92b8-bbfa183b7392"),
                        }
                    ]),
                default
            },
            {
                new(
                    value:
                    [
                        new()
                        {
                            Id = new("4a3b89d5-0086-40a5-991b-cc7861d2a0f4")
                        }
                    ],
                    nextLink: string.Empty),
                default
            },
            {
                new(
                    value: default,
                    nextLink: "Some Link"),
                new()
                {
                    HasMore = true
                }
            },
            {
                new(
                    value:
                    [
                        new()
                        {
                            Id = new("02e072d5-127e-447b-92b8-bbfa183b7392")
                        },
                        new()
                        {
                            Id = new("02e072d5-127e-447b-92b8-bbfa183b7392"),
                        }
                    ],
                    nextLink: "http://api.example.com/customers?page=2"),
                new()
                {
                    HasMore = true
                }
            },
        };
}
