using System.Xml.Linq;
using XmlTest.Models;

namespace XmlTest.Services;

public class XmlDocumentService : IXmlExportService, IXmlImportService
{
    private static class XmlNames
    {
        public const string Root = "NixZdDocument";
        public const string Records = "Records";
        public const string Record = "Record";
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Value = "Value";
        public const string Timestamp = "Timestamp";
        public const string Version = "Version";
        public const string Created = "Created";
    }

    public XDocument ExportToXDocument(NixZdDocument document)
    {
        var root = new XElement(XmlNames.Root,
            new XElement(XmlNames.Version, document.Version),
            new XElement(XmlNames.Created, document.CreatedText),
            new XElement(XmlNames.Records,
                document.Records.Select(r => new XElement(XmlNames.Record,
                    new XElement(XmlNames.Id, r.Id),
                    new XElement(XmlNames.Name, r.Name),
                    new XElement(XmlNames.Value, r.ValueText),
                    new XElement(XmlNames.Timestamp, r.TimestampText)
                ))
            )
        );
        return new XDocument(new XDeclaration("1.0", "utf-8", null), root);
    }

    public void ExportToFile(NixZdDocument document, string filePath)
    {
        var doc = ExportToXDocument(document);
        doc.Save(filePath);
    }

    public NixZdDocument ImportFromFile(string filePath)
    {
        var doc = XDocument.Load(filePath);
        return ImportFromXDocument(doc);
    }

    public NixZdDocument ImportFromXDocument(XDocument doc)
    {
        var root = doc.Root ?? throw new InvalidOperationException("XML document has no root element.");
        if (root.Name.LocalName != XmlNames.Root)
            throw new InvalidOperationException($"Expected root element '{XmlNames.Root}', got '{root.Name.LocalName}'.");

        var version = root.Element(XmlNames.Version)?.Value ?? "1.0";
        var createdEl = root.Element(XmlNames.Created);
        var recordsEl = root.Element(XmlNames.Records);

        var document = new NixZdDocument { Version = version };
        if (createdEl != null && !string.IsNullOrEmpty(createdEl.Value))
            document.CreatedText = createdEl.Value;

        if (recordsEl != null)
        {
            foreach (var recEl in recordsEl.Elements(XmlNames.Record))
            {
                var record = new MeasurementRecord
                {
                    Id = int.Parse(recEl.Element(XmlNames.Id)?.Value ?? "0"),
                    Name = recEl.Element(XmlNames.Name)?.Value ?? string.Empty
                };
                var valueEl = recEl.Element(XmlNames.Value);
                if (valueEl != null && !string.IsNullOrEmpty(valueEl.Value))
                    record.ValueText = valueEl.Value;
                var timestampEl = recEl.Element(XmlNames.Timestamp);
                if (timestampEl != null && !string.IsNullOrEmpty(timestampEl.Value))
                    record.TimestampText = timestampEl.Value;
                document.Records.Add(record);
            }
        }

        return document;
    }
}
