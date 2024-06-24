using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class EmployeeCostSetGetHandler
{
    public ValueTask<Result<EmployeeCostSetGetOut, Failure<HandlerFailureCode>>> HandleAsync(
        EmployeeCostSetGetIn input, CancellationToken cancellationToken)
        =>
        new(
            result: new EmployeeCostSetGetOut
            {
                EmployeeCostItems =
                [
                    new(Guid.NewGuid(), "Employee First", 1000),
                    new(Guid.NewGuid(), "Employee Second", 1500),
                    new(Guid.NewGuid(), "Employee Third", 1300),
                    new(Guid.NewGuid(), "Employee Fourth", 2000),
                    new(Guid.NewGuid(), "Employee Fifth", 2100),
                    new(Guid.NewGuid(), "Employee Sixth", 800),
                    new(Guid.NewGuid(), "Employee Seventh", 1000),
                    new(Guid.NewGuid(), "Employee Eights", 3000),
                    new(Guid.NewGuid(), "Employee Ninth", 250),
                    new(Guid.NewGuid(), "Employee Tenth", 2570)
                ]
            });
}