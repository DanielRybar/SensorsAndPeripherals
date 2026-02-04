using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Interfaces
{
    public interface IGeolocationService
    {
        Task<(LocationStatus status, Location? location)> GetCachedLocation();
        Task<(LocationStatus status, Location? location)> GetCurrentLocation();
    }
}