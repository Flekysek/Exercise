using System.Data;
using XmlTest.Models;

namespace XmlTest.Data;

public class RecordRepository : IRecordRepository
{
    public Task SaveRecordsAsync(IEnumerable<MeasurementRecord> records, CancellationToken cancellationToken = default)
    {
        var table = DataTableSchema.CreateRecordsTable();
        DataTableSchema.FillFromRecords(table, records);
            
        // TODO:
        // Příprava na zápis do DB – connection a commit zatím nejsou k dispozici
        // var connectionString = "Server=...;Database=...;";
        // using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        // await connection.OpenAsync(cancellationToken);
        // using var adapter = new Microsoft.Data.SqlClient.SqlDataAdapter(...);
        // adapter.Update(table);
        // connection.Commit();

        return Task.CompletedTask;
    }
}
