using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class ProjectCostSetDeleteHandler : IProjectCostSetDeleteHandler
{
    private readonly IDataverseApiClient dataverseApi;

    internal ProjectCostSetDeleteHandler(IDataverseApiClient dataverseApi)
        =>
        this.dataverseApi = dataverseApi;
}