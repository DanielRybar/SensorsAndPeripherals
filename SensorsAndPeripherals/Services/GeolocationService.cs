using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Services
{
    public class GeolocationService : IGeolocationService
    {
        private CancellationTokenSource? cancelTokenSource;

        public async Task<(LocationStatus status, Location? location)> GetLastKnownCachedLocationAsync()
        {
            return await GetLocation(fromCache: true);
        }

        public async Task<(LocationStatus status, Location? location)> GetCurrentFineLocationAsync()
        {
            return await GetLocation(fromCache: false, isFine: true);
        }

        public async Task<(LocationStatus status, Location? location)> GetCurrentCoarseLocationAsync()
        {
            return await GetLocation(fromCache: false, isFine: false);
        }

        public async Task<Placemark?> GetPlacemarkFromCoordinatesAsync(double latitude, double longitude)
        {
            try
            {
                var placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);
                var placemark = placemarks?.FirstOrDefault();
                if (placemark is not null)
                {
                    return placemark;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public void CancelCurrentLocationRequest()
        {
            if (cancelTokenSource is not null && !cancelTokenSource.IsCancellationRequested)
            {
                cancelTokenSource?.Cancel();
            }
        }

        private async Task<(LocationStatus status, Location? location)> GetLocation(bool fromCache, bool isFine = true)
        {
            try
            {
                Location? location = null;
                if (fromCache)
                {
                    location = await Geolocation.Default.GetLastKnownLocationAsync();
                }
                else
                {
                    CancelCurrentLocationRequest();
                    var request = new GeolocationRequest(isFine ? GeolocationAccuracy.Best : GeolocationAccuracy.Low, TimeSpan.FromSeconds(10));
                    cancelTokenSource = new CancellationTokenSource();
                    location = await Geolocation.Default.GetLocationAsync(request, cancelTokenSource.Token);
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
            catch (OperationCanceledException)
            {
                return (LocationStatus.OperationCancelled, null);
            }
            catch (Exception)
            {
                return (LocationStatus.UnknownError, null);
            }
            finally
            {
                if (!fromCache && cancelTokenSource is not null)
                {
                    cancelTokenSource.Dispose();
                    cancelTokenSource = null;
                }
            }
        }
    }
}