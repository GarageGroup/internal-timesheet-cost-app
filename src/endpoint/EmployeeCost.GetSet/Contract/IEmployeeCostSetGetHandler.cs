using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

public interface IEmployeeCostSetGetHandler : IHandler<EmployeeCostSetGetIn, EmployeeCostSetGetOut>
{
    public const string FunctionName = "GetEmployeeCosts";
}