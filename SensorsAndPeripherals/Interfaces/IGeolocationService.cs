using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Interfaces
{
    public interface IGeolocationService
    {
        Task<(LocationStatus status, Location? location)> GetLastKnownCachedLocationAsync();
        Task<(LocationStatus status, Location? location)> GetCurrentFineLocationAsync();
        Task<(LocationStatus status, Location? location)> GetCurrentCoarseLocationAsync();
        Task<Placemark?> GetPlacemarkFromCoordinatesAsync(double latitude, double longitude);
        void CancelCurrentLocationRequest();
    }
}