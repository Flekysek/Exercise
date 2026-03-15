using System.Data;
using XmlTest.Models;

namespace XmlTest.Data;

public static class DataTableSchema
{
    public const string TableName = "MeasurementRecords";
    public const string ColumnId = "Id";
    public const string ColumnName = "Name";
    public const string ColumnValue = "Value";
    public const string ColumnTimestamp = "Timestamp";

    public static DataTable CreateRecordsTable()
    {
        var table = new DataTable(TableName);
        table.Columns.Add(ColumnId, typeof(int));
        table.Columns.Add(ColumnName, typeof(string));
        table.Columns.Add(ColumnValue, typeof(double));
        table.Columns.Add(ColumnTimestamp, typeof(DateTime));
        return table;
    }

    public static void FillFromRecords(DataTable table, IEnumerable<MeasurementRecord> records)
    {
        foreach (var r in records)
        {
            var row = table.NewRow();
            row[ColumnId] = r.Id;
            row[ColumnName] = r.Name;
            row[ColumnValue] = r.Value;
            row[ColumnTimestamp] = r.Timestamp;
            table.Rows.Add(row);
        }
    }
}
