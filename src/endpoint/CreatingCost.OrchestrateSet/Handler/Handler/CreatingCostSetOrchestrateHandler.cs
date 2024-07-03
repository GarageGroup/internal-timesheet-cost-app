using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class CreatingCostSetOrchestrateHandler : ICreatingCostSetOrchestrateHandler
{
    private const int MaxCostItems = 32;

    private readonly IOrchestrationActivityApi orchestrationActivityApi;

    internal CreatingCostSetOrchestrateHandler(IOrchestrationActivityApi orchestrationActivityApi)
        =>
        this.orchestrationActivityApi = orchestrationActivityApi;

    private static FlatArray<EmployeeCost> MapEmployeeCosts(CreatingCostSetOrchestrateIn input, FlatArray<EmployeeCostItem> costs)
    {
        return costs.Map(MapItem);

        EmployeeCost MapItem(EmployeeCostItem item)
            =>
            new()
            {
                CostPeriodId = input.CostPeriodId,
                SystemUserId = item.SystemUserId,
                CallerUserId = input.CallerUserId,
                Cost = item.EmployeeCost
            };
    }

    private sealed record class EmployeeCost
    {
        public required Guid CostPeriodId { get; init; }

        public required Guid SystemUserId { get; init; }

        public required Guid CallerUserId { get; init; }

        public required decimal Cost { get; init; }
    }
}