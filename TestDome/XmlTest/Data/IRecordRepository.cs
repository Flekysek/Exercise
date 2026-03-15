using XmlTest.Models;

namespace XmlTest.Data;

public interface IRecordRepository
{
    Task SaveRecordsAsync(IEnumerable<MeasurementRecord> records, CancellationToken cancellationToken = default);
}
