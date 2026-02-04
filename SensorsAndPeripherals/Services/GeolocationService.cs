using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Services
{
    public class GeolocationService : IGeolocationService
    {
        public async Task<(LocationStatus status, Location? location)> GetCachedLocation()
        {
            return await GetLocation(isFine: false);
        }

        public async Task<(LocationStatus status, Location? location)> GetCurrentLocation()
        {
            return await GetLocation(isFine: true);
        }

        private static async Task<(LocationStatus status, Location? location)> GetLocation(bool isFine)
        {
            try
            {
                Location? location = null;
                if (isFine)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    location = await Geolocation.Default.GetLocationAsync(request, CancellationToken.None);
                }
                else
                {
                    location = await Geolocation.Default.GetLastKnownLocationAsync();
                }
                if (location is not null)
                {
                    return (LocationStatus.Obtained, location);
                }
                return (LocationStatus.ObtainedButNull, null);
            }
            catch (FeatureNotSupportedException)
            {
                return (LocationStatus.NotSupported, null);
            }
            catch (FeatureNotEnabledException)
            {
                return (LocationStatus.NotEnabled, null);
            }
            catch (PermissionException)
            {
                return (LocationStatus.PermissionDenied, null);
            }
            catch (Exception)
            {
                return (LocationStatus.UnknownError, null);
            }
        }
    }
}