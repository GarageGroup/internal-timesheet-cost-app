using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class ProjectCostSetDeleteHandler(IDataverseApiClient dataverseApi) : IProjectCostSetDeleteHandler
{
    private sealed record class EmployeeProjectCostModel
    {
        public EmployeeProjectCostModel(
            DataverseEntitySetGetOut<EmployeeProjectCostJson> employeeProjectCosts,
            Guid systemUserId)
        {
            EmployeeProjectCosts = employeeProjectCosts;
            SystemUserId = systemUserId;
        }

        public DataverseEntitySetGetOut<EmployeeProjectCostJson> EmployeeProjectCosts { get; }

        public Guid SystemUserId { get; }
    }

    private sealed record class DeleteEmployeeProjectCostModel
    {
        public DeleteEmployeeProjectCostModel(
            Guid employeeProjectCostId,
            Guid systemUserId)
        {
            EmployeeProjectCostId = employeeProjectCostId;
            SystemUserId = systemUserId;
        }

        public Guid EmployeeProjectCostId { get; }

        public Guid SystemUserId { get; }
    }
}