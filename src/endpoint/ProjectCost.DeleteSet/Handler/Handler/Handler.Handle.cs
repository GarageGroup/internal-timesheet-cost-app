using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class ProjectCostSetDeleteHandler
{
    public ValueTask<Result<ProjectCostSetDeleteOut, Failure<HandlerFailureCode>>> HandleAsync(
        ProjectCostSetDeleteIn input, CancellationToken cancellationToken)
        =>
        new(Result.Success<ProjectCostSetDeleteOut>(default));
}