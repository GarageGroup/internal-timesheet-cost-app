using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class ProjectCostSetDeleteHandler(IDataverseApiClient dataverseApi) : IProjectCostSetDeleteHandler
{
    private sealed record class EmployeeProjectCostModel
    {
        public EmployeeProjectCostModel(
            DataverseEntitySetGetOut<EmployeeProjectCostJson> employeeProjectCosts,
            Guid callerUserId)
        {
            EmployeeProjectCosts = employeeProjectCosts;
            CallerUserId = callerUserId;
        }

        public DataverseEntitySetGetOut<EmployeeProjectCostJson> EmployeeProjectCosts { get; }

        public Guid CallerUserId { get; }
    }

    private sealed record class DeleteEmployeeProjectCostModel
    {
        public DeleteEmployeeProjectCostModel(
            Guid employeeProjectCostId,
            Guid callerUserId)
        {
            EmployeeProjectCostId = employeeProjectCostId;
            CallerUserId = callerUserId;
        }

        public Guid EmployeeProjectCostId { get; }

        public Guid CallerUserId { get; }
    }
}