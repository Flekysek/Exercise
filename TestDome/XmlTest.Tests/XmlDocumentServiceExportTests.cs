using System.Xml.Linq;
using XmlTest.Models;
using XmlTest.Services;
using Xunit;

namespace XmlTest.Tests;

public class XmlDocumentServiceExportTests
{
    private static NixZdDocument CreateSampleDocumentWithOneRecord()
    {
        var created = new DateTime(2025, 3, 15, 10, 30, 0, DateTimeKind.Utc);
        return new NixZdDocument
        {
            Version = "1.0",
            Created = created,
            Records =
            {
                new MeasurementRecord
                {
                    Id = 1,
                    Name = "Test",
                    Value = 2.5,
                    Timestamp = new DateTime(2025, 3, 15, 9, 0, 0, DateTimeKind.Utc)
                }
            }
        };
    }

    [Fact]
    public void ExportToXDocument_ReturnsNonNullXDocumentWithNixZdDocumentRoot()
    {
        var service = new XmlDocumentService();
        var document = CreateSampleDocumentWithOneRecord();

        var result = service.ExportToXDocument(document);

        Assert.NotNull(result);
        Assert.NotNull(result.Root);
        Assert.Equal("NixZdDocument", result.Root.Name.LocalName);
    }

    [Fact]
    public void ExportToXDocument_RootContainsVersionCreatedAndRecords()
    {
        var service = new XmlDocumentService();
        var document = CreateSampleDocumentWithOneRecord();

        var result = service.ExportToXDocument(document);
        var root = result.Root!;

        Assert.NotNull(root.Element("Version"));
        Assert.NotNull(root.Element("Created"));
        Assert.NotNull(root.Element("Records"));
    }

    [Fact]
    public void ExportToXDocument_SingleRecord_ContainsIdNameValueTimestampWithCorrectValues()
    {
        var service = new XmlDocumentService();
        var document = CreateSampleDocumentWithOneRecord();

        var result = service.ExportToXDocument(document);
        var records = result.Root!.Element("Records")!;
        var record = records.Elements("Record").Single();

        Assert.Equal("1", record.Element("Id")!.Value);
        Assert.Equal("Test", record.Element("Name")!.Value);
        Assert.Equal("2.5", record.Element("Value")!.Value);
        // DateTime round-trip format (ISO 8601)
        Assert.Contains("2025-03-15", record.Element("Timestamp")!.Value);
        Assert.Contains("09:00:00", record.Element("Timestamp")!.Value);
    }

    [Fact]
    public void ExportToXDocument_DateTimeInRoundTripFormat_DoubleInInvariantFormat()
    {
        var service = new XmlDocumentService();
        var document = CreateSampleDocumentWithOneRecord();
        document.Records[0].ValueText = "3.14159";

        var result = service.ExportToXDocument(document);
        var record = result.Root!.Element("Records")!.Elements("Record").Single();

        Assert.Equal("3.14159", record.Element("Value")!.Value);
        // Round-trip contains 'Z' for UTC
        var timestamp = record.Element("Timestamp")!.Value;
        Assert.True(timestamp.EndsWith("Z") || timestamp.Contains("+"), "Timestamp should be ISO 8601 round-trip.");
    }

    [Fact]
    public void ExportToFile_WritesValidXmlThatCanBeLoadedAndImported()
    {
        var service = new XmlDocumentService();
        var document = CreateSampleDocumentWithOneRecord();
        var path = Path.GetTempFileName();
        try
        {
            service.ExportToFile(document, path);

            var loaded = XDocument.Load(path);
            Assert.NotNull(loaded.Root);
            Assert.Equal("NixZdDocument", loaded.Root.Name.LocalName);

            var imported = service.ImportFromFile(path);
            Assert.Equal(document.Version, imported.Version);
            Assert.Single(imported.Records);
            Assert.Equal(1, imported.Records[0].Id);
            Assert.Equal("Test", imported.Records[0].Name);
            Assert.Equal(2.5, imported.Records[0].Value, precision: 6);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
