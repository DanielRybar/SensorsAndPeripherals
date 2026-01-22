namespace SensorsAndPeripherals.Models
{
    public class SensorInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Vendor { get; set; } = string.Empty;
        public int Version { get; set; }
        public float Power { get; set; }
        public string StringType { get; set; } = string.Empty;
    }
}