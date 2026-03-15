using XmlTest.Data;
using XmlTest.Models;
using XmlTest.Services;

// Export data
var directRecord = new MeasurementRecord
{
    Id = 0,
    Name = "Direct sample",
    Value = 3.14,
    Timestamp = new DateTime(2025, 3, 15, 10, 0, 0, DateTimeKind.Utc)
};

var document = new NixZdDocument
{
    Version = "1.0",
    Created = DateTime.UtcNow,
    Records =
    {
        directRecord,
        new MeasurementRecord
        {
            Id = 1,
            Name = "Temperature",
            Value = 22.5,
            Timestamp = new DateTime(2025, 3, 15, 9, 30, 0, DateTimeKind.Utc)
        },
        new MeasurementRecord
        {
            Id = 2,
            Name = "Pressure",
            Value = 1013.25,
            Timestamp = new DateTime(2025, 3, 15, 9, 45, 0, DateTimeKind.Utc)
        }
    }
};

const string outputPath = "xmlOutput.xml";
var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
var downloadsPath = Path.Combine(userProfile, "Downloads", outputPath);
var xmlService = new XmlDocumentService();

// Export do XML
xmlService.ExportToFile(document, downloadsPath);
Console.WriteLine($"Exported to {downloadsPath}");

// Čtení ze souboru
var imported = xmlService.ImportFromFile(downloadsPath);
Console.WriteLine($"Imported document Version={imported.Version}, Records={imported.Records.Count}");

// Příprava zápisu do DB (DataTable naplněn; connection a commit zakomentované v RecordRepository)
var repository = new RecordRepository();
await repository.SaveRecordsAsync(imported.Records);
Console.WriteLine("Records prepared for DB (DataTable filled; connection/commit commented out).");
