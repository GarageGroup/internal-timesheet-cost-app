using System;

namespace GarageGroup.Internal.Timesheet;

partial record class DbTimesheet
{
    internal static readonly FlatArray<string> DefaultGroups
        =
        [
            RegardingObjectIdFieldName,
            RegardingObjectTypeCodeFieldName
        ];
}