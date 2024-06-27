using GarageGroup.Infra;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GarageGroup.Internal.Timesheet.Cost.Endpoint.CreatingCost.OrchestrateSet.Test;

public static partial class CreatingCostOrchestrateHandlerTest
{
    private static readonly OrchestrationActivityCallOut<ProjectCostSetDeleteOut> SomeDeleteOut
        =
        new(
            value: new()
            {
                HasMore = false
            });

    private static readonly OrchestrationActivityCallOut<EmployeeCostSetGetOut> SomeSetGetOut
        =
        new(
            value: new()
            {
                EmployeeCostItems =
                [
                    new(
                        systemUserId: new("69a4ea6f-6e12-4490-bf14-b5b604200450"),
                        employeeCost: 100m),
                    new(
                        systemUserId: new("4d6d634e-523a-4bf8-bf51-3a13b06384b1"),
                        employeeCost: 200m)
                ]
            });

    private static readonly CreatingCostSetOrchestrateIn SomeInput
        =
        new(
            new("32ad38e2-abef-49e5-9db7-1258b195a7c3"));

    private static Mock<IOrchestrationActivityApi> BuildMockOrchestration(
        in Result<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>, Failure<HandlerFailureCode>> deleteResult,
        in Result<OrchestrationActivityCallOut<EmployeeCostSetGetOut>, Failure<HandlerFailureCode>> setGetResult,
        in Result<Unit, Failure<HandlerFailureCode>> createResult)
    {
        var mock = new Mock<IOrchestrationActivityApi>();

        _ = mock
            .Setup(static a => a.CallActivityAsync<ProjectCostSetDeleteIn, ProjectCostSetDeleteOut>(It.IsAny<OrchestrationActivityCallIn<ProjectCostSetDeleteIn>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(deleteResult);

        _ = mock
            .Setup(static a => a.CallActivityAsync<EmployeeCostSetGetIn, EmployeeCostSetGetOut>(It.IsAny<OrchestrationActivityCallIn<EmployeeCostSetGetIn>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(setGetResult);

        _ = mock
            .Setup(static a => a.CallActivityAsync(It.IsAny<OrchestrationActivityCallIn<ProjectCostSetCreateIn>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createResult);

        return mock;
    }

    private static Mock<IOrchestrationActivityApi> BuildMockOrchestrationForDeleteTest(
        in FlatArray<Result<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>, Failure<HandlerFailureCode>>> deleteResultSet)
    {
        var queue = new Queue<Result<OrchestrationActivityCallOut<ProjectCostSetDeleteOut>, Failure<HandlerFailureCode>>>(deleteResultSet.AsEnumerable());

        var mock = new Mock<IOrchestrationActivityApi>();

        _ = mock
            .Setup(static a => a.CallActivityAsync<ProjectCostSetDeleteIn, ProjectCostSetDeleteOut>(It.IsAny<OrchestrationActivityCallIn<ProjectCostSetDeleteIn>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queue.Dequeue);

        var setGetResult = new OrchestrationActivityCallOut<EmployeeCostSetGetOut>(
            value: new()
            {
                EmployeeCostItems =
                [
                    new(
                        systemUserId: new("69a4ea6f-6e12-4490-bf14-b5b604200450"),
                        employeeCost: 100m),
                    new(
                        systemUserId: new("4d6d634e-523a-4bf8-bf51-3a13b06384b1"),
                        employeeCost: 200m)
                ]
            });

        _ = mock
            .Setup(static a => a.CallActivityAsync<EmployeeCostSetGetIn, EmployeeCostSetGetOut>(It.IsAny<OrchestrationActivityCallIn<EmployeeCostSetGetIn>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(setGetResult);

        _ = mock
            .Setup(static a => a.CallActivityAsync(It.IsAny<OrchestrationActivityCallIn<ProjectCostSetCreateIn>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<Unit>(default));

        return mock;
    }
}