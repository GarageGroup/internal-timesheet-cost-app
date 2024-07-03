using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class ProjectCostSetDeleteHandler(IDataverseApiClient dataverseApi) : IProjectCostSetDeleteHandler
{
    private static FlatArray<DeleteEmployeeProjectCostModel> GetEmployeeProjectCosts(EmployeeProjectCostModel model)
    {
        return model.EmployeeProjectCosts.Value.Map(Map);

        DeleteEmployeeProjectCostModel Map(EmployeeProjectCostJson json)
            =>
            new()
            {
                EmployeeProjectCostId = json.Id,
                CallerUserId = model.CallerUserId
            };
    }

    private static HandlerFailureCode MapFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.UserNotEnabled => HandlerFailureCode.Persistent,
            DataverseFailureCode.PrivilegeDenied => HandlerFailureCode.Persistent,
            _ => HandlerFailureCode.Transient
        };

    private sealed record class EmployeeProjectCostModel
    {
        public required DataverseEntitySetGetOut<EmployeeProjectCostJson> EmployeeProjectCosts { get; init; }

        public required Guid CallerUserId { get; init; }
    }

    private sealed record class DeleteEmployeeProjectCostModel
    {
        public required Guid EmployeeProjectCostId { get; init; }

        public required Guid CallerUserId { get; init; }
    }
}