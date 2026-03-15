using System.Globalization;

namespace XmlTest.Models;

public class MeasurementRecord
{
    private double _value;
    private DateTime _timestamp;

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Value
    {
        get => _value;
        init
        {
            _value = value;
            ValueText = value.ToString(CultureInfo.InvariantCulture);
        }
    }
    public string ValueText
    {
        get => _value.ToString(CultureInfo.InvariantCulture);
        set => _value = double.Parse(value, CultureInfo.InvariantCulture);
    }
    public DateTime Timestamp
    {
        get => _timestamp;
        init
        {
            _timestamp = value;
            TimestampText = value.ToString("o");
        }
    }
    public string TimestampText
    {
        get => _timestamp.ToString("o");
        set => _timestamp = DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }
}
