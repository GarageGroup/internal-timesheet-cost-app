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

    private static FlatArray<EmployeeCost> MapEmployeeCosts(Guid costPeriodId, FlatArray<EmployeeCostItem> costs)
    {
        return costs.Map(MapItem);

        EmployeeCost MapItem(EmployeeCostItem item)
            =>
            new()
            {
                CostPeriodId = costPeriodId,
                SystemUserId = item.SystemUserId,
                EmployeeName = item.EmployeeName,
                Cost = item.EmployeeCost
            };
    }

    private sealed record class EmployeeCost
    {
        public Guid CostPeriodId { get; init; }

        public Guid SystemUserId { get; init; }

        public string? EmployeeName { get; init; }

        public decimal Cost { get; init; }
    }
}