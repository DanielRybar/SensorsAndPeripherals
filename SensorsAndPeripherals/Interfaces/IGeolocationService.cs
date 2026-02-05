using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Interfaces
{
    public interface IGeolocationService
    {
        Task<(LocationStatus status, Location? location)> GetLastKnownCachedLocation();
        Task<(LocationStatus status, Location? location)> GetCurrentFineLocation();
        Task<(LocationStatus status, Location? location)> GetCurrentCoarseLocation();
        Task<Placemark?> GetPlacemarkFromCoordinates(double latitude, double longitude);
        void CancelCurrentLocationRequest();
    }
}