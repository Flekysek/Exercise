using XmlTest.Models;
using XmlTest.Services;
using Xunit;

namespace XmlTest.Tests;

public class XmlDocumentServiceRoundTripTests
{
    private static NixZdDocument CreateSampleDocumentWithMultipleRecords()
    {
        return new NixZdDocument
        {
            Version = "1.0",
            Created = new DateTime(2025, 3, 15, 12, 0, 0, DateTimeKind.Utc),
            Records =
            {
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
    }

    [Fact]
    public void RoundTrip_ExportThenImport_PreservesVersionAndCreated()
    {
        var original = CreateSampleDocumentWithMultipleRecords();
        var service = new XmlDocumentService();

        var doc = service.ExportToXDocument(original);
        var imported = service.ImportFromXDocument(doc);

        Assert.Equal(original.Version, imported.Version);
        Assert.Equal(original.CreatedText, imported.CreatedText);
    }

    [Fact]
    public void RoundTrip_ExportThenImport_PreservesRecordCountAndData()
    {
        var original = CreateSampleDocumentWithMultipleRecords();
        var service = new XmlDocumentService();

        var doc = service.ExportToXDocument(original);
        var imported = service.ImportFromXDocument(doc);

        Assert.Equal(original.Records.Count, imported.Records.Count);

        for (var i = 0; i < original.Records.Count; i++)
        {
            Assert.Equal(original.Records[i].Id, imported.Records[i].Id);
            Assert.Equal(original.Records[i].Name, imported.Records[i].Name);
            Assert.Equal(original.Records[i].Value, imported.Records[i].Value, precision: 6);
            Assert.Equal(original.Records[i].Timestamp, imported.Records[i].Timestamp);
        }
    }

    [Fact]
    public void RoundTrip_ExportToFileThenImportFromFile_PreservesData()
    {
        var original = CreateSampleDocumentWithMultipleRecords();
        var service = new XmlDocumentService();
        var path = Path.GetTempFileName();
        try
        {
            service.ExportToFile(original, path);
            var imported = service.ImportFromFile(path);

            Assert.Equal(original.Version, imported.Version);
            Assert.Equal(original.CreatedText, imported.CreatedText);
            Assert.Equal(original.Records.Count, imported.Records.Count);
            for (var i = 0; i < original.Records.Count; i++)
            {
                Assert.Equal(original.Records[i].Id, imported.Records[i].Id);
                Assert.Equal(original.Records[i].Name, imported.Records[i].Name);
                Assert.Equal(original.Records[i].Value, imported.Records[i].Value, precision: 6);
                Assert.Equal(original.Records[i].Timestamp, imported.Records[i].Timestamp);
            }
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
