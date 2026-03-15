using System.Globalization;

namespace XmlTest.Models;

public class NixZdDocument
{
    private DateTime _created;

    public string Version { get; init; } = "1.0";
    public DateTime Created
    {
        get => _created;
        set
        {
            _created = value;
            CreatedText = value.ToString("o");
        }
    }
    public string CreatedText
    {
        get => _created.ToString("o");
        set => _created = DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }
    public List<MeasurementRecord> Records { get; set; } = [];
}
