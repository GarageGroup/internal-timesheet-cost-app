using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class EmployeeCostSetGetHandler : IEmployeeCostSetGetHandler
{
    private readonly ISqlQueryEntitySetSupplier sqlApi;

    internal EmployeeCostSetGetHandler(ISqlQueryEntitySetSupplier sqlApi)
        =>
        this.sqlApi = sqlApi;
}