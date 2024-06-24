using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class ProjectCostSetCreateHandler : IProjectCostSetCreateHandler
{
    private readonly ISqlQueryEntitySetSupplier sqlApi;

    private readonly IDataverseEntityCreateSupplier dataverseApi;

    internal ProjectCostSetCreateHandler(ISqlQueryEntitySetSupplier sqlApi, IDataverseEntityCreateSupplier dataverseApi)
    {
        this.sqlApi = sqlApi;
        this.dataverseApi = dataverseApi;
    }
}