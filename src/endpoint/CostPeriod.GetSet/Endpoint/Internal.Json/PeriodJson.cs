using System;
using System.Text.Json.Serialization;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed record class PeriodJson
{
    private const string EntityPluralName
        =
        "gg_employee_cost_periods";

    private const string IdFieldName
        =
        "gg_employee_cost_periodid";

    private const string NameFieldName
        =
        "gg_name";

    private const string FromDateFieldName
        =
        "gg_from_date";

    private const string ToDateFieldName
        =
        "gg_to_date";

    internal static DataverseEntitySetGetIn BuildDataverseSetGetInput()
        =>
        new(
            entityPluralName: EntityPluralName,
            selectFields: [IdFieldName, NameFieldName, FromDateFieldName, ToDateFieldName],
            filter: default,
            expandFields: default,
            orderBy: 
            [
                new(
                    fieldName: ToDateFieldName, 
                    direction: DataverseOrderDirection.Descending)
            ]);

    [JsonPropertyName(IdFieldName)]
    public Guid Id { get; init; }

    [JsonPropertyName(NameFieldName)]
    public string? Name { get; init; }

    [JsonPropertyName(FromDateFieldName)]
    public DateTime From { get; init; }

    [JsonPropertyName(ToDateFieldName)]
    public DateTime To { get; init; }
}