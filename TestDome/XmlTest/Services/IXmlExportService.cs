using System.Xml.Linq;
using XmlTest.Models;

namespace XmlTest.Services;

public interface IXmlExportService
{
    XDocument ExportToXDocument(NixZdDocument document);
    void ExportToFile(NixZdDocument document, string filePath);
}
