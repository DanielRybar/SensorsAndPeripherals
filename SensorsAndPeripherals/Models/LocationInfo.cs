using SensorsAndPeripherals.Models.Abstract;

namespace SensorsAndPeripherals.Models
{
    public class LocationInfo : BaseModel
    {
        public double Latitude { get; set => SetProperty(ref field, value); }
        public double Longitude { get; set => SetProperty(ref field, value); }
        public double? Altitude { get; set => SetProperty(ref field, value); }
        public double? Accuracy { get; set => SetProperty(ref field, value); }
        public DateTimeOffset Timestamp { get; set => SetProperty(ref field, value); }
    }
}