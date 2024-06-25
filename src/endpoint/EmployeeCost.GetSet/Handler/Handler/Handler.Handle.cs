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
                    new(Guid.NewGuid(), 1000),
                    new(Guid.NewGuid(), 1500),
                    new(Guid.NewGuid(), 1300),
                    new(Guid.NewGuid(), 2000),
                    new(Guid.NewGuid(), 2100),
                    new(Guid.NewGuid(), 800),
                    new(Guid.NewGuid(), 1000),
                    new(Guid.NewGuid(), 3000),
                    new(Guid.NewGuid(), 250),
                    new(Guid.NewGuid(), 2570)
                ]
            });
}