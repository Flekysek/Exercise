using System.Xml.Linq;
using XmlTest.Models;

namespace XmlTest.Services;

public interface IXmlImportService
{
    NixZdDocument ImportFromFile(string filePath);
    NixZdDocument ImportFromXDocument(XDocument doc);
}
