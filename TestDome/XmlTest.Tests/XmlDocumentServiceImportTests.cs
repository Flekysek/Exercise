using System.Xml.Linq;
using XmlTest.Models;
using XmlTest.Services;
using Xunit;

namespace XmlTest.Tests;

public class XmlDocumentServiceImportTests
{
    private const string ValidMinimalXml = """
                                           <?xml version="1.0" encoding="utf-8"?>
                                           <NixZdDocument>
                                             <Version>1.0</Version>
                                             <Created>2025-03-15T10:30:00.0000000Z</Created>
                                             <Records>
                                               <Record>
                                                 <Id>1</Id>
                                                 <Name>Test</Name>
                                                 <Value>2.5</Value>
                                                 <Timestamp>2025-03-15T09:00:00.0000000Z</Timestamp>
                                               </Record>
                                             </Records>
                                           </NixZdDocument>
                                           """;

    [Fact]
    public void ImportFromXDocument_ValidDocument_ReturnsDocumentWithVersionAndOneRecord()
    {
        var doc = XDocument.Parse(ValidMinimalXml);
        var service = new XmlDocumentService();

        var result = service.ImportFromXDocument(doc);

        Assert.Equal("1.0", result.Version);
        Assert.Single(result.Records);
        Assert.Equal(1, result.Records[0].Id);
        Assert.Equal("Test", result.Records[0].Name);
        Assert.Equal(2.5, result.Records[0].Value, precision: 6);
        Assert.Equal(new DateTime(2025, 3, 15, 9, 0, 0, DateTimeKind.Utc), result.Records[0].Timestamp);
    }

    [Fact]
    public void ImportFromXDocument_EmptyRecords_ReturnsDocumentWithZeroRecords()
    {
        var xml = """
            <?xml version="1.0" encoding="utf-8"?>
            <NixZdDocument>
              <Version>2.0</Version>
              <Created>2025-03-15T10:30:00Z</Created>
              <Records />
            </NixZdDocument>
            """;
        var doc = XDocument.Parse(xml);
        var service = new XmlDocumentService();

        var result = service.ImportFromXDocument(doc);

        Assert.Equal("2.0", result.Version);
        Assert.Empty(result.Records);
    }

    [Fact]
    public void ImportFromXDocument_NullRoot_ThrowsInvalidOperationException()
    {
        var doc = new XDocument();
        var service = new XmlDocumentService();

        var ex = Assert.Throws<InvalidOperationException>(() => service.ImportFromXDocument(doc));
        Assert.Contains("no root", ex.Message);
    }

    [Fact]
    public void ImportFromXDocument_WrongRootName_ThrowsInvalidOperationException()
    {
        var xml = "<?xml version=\"1.0\"?><OtherRoot><Version>1.0</Version></OtherRoot>";
        var doc = XDocument.Parse(xml);
        var service = new XmlDocumentService();

        var ex = Assert.Throws<InvalidOperationException>(() => service.ImportFromXDocument(doc));
        Assert.Contains("NixZdDocument", ex.Message);
    }

    [Fact]
    public void ImportFromFile_ValidFile_ReturnsSameResultAsImportFromXDocument()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, ValidMinimalXml);
            var service = new XmlDocumentService();

            var fromFile = service.ImportFromFile(path);
            var fromDoc = service.ImportFromXDocument(XDocument.Parse(ValidMinimalXml));

            Assert.Equal(fromDoc.Version, fromFile.Version);
            Assert.Equal(fromDoc.Records.Count, fromFile.Records.Count);
            if (fromDoc.Records.Count > 0)
            {
                Assert.Equal(fromDoc.Records[0].Id, fromFile.Records[0].Id);
                Assert.Equal(fromDoc.Records[0].Name, fromFile.Records[0].Name);
                Assert.Equal(fromDoc.Records[0].Value, fromFile.Records[0].Value, precision: 6);
            }
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void ImportFromFile_NonExistentFile_ThrowsFileNotFoundException()
    {
        var service = new XmlDocumentService();
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xml");

        Assert.Throws<FileNotFoundException>(() => service.ImportFromFile(path));
    }
}
