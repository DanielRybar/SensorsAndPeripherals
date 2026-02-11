using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Interfaces.Sensors;
using SensorsAndPeripherals.Models;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;
using LocationStatus = SensorsAndPeripherals.Models.Enums.LocationStatus;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class GpsViewModel : BaseViewModel
    {
        #region services
        private readonly IGpsService gpsService = DependencyService.Get<IGpsService>();
        #endregion

        #region constructor
        public GpsViewModel()
        {
            GetLastKnownCachedLocationCommand = new Command(async () =>
            {
                await GetLocation(fromCache: true);
            });
            GetCurrentFineLocationCommand = new Command(async () =>
            {
                await GetLocation(fromCache: false, isFine: true);
            });
            GetCurrentCoarseLocationCommand = new Command(async () =>
            {
                await GetLocation(fromCache: false, isFine: false);
            });
            GetAddressFromCoordinatesCommand = new Command(() =>
            {
                ShowAddressDialogRequested?.Invoke(ResultLocation?.Address ?? "GpsAddressNotAvailable".GetStringFromResource());
            },
            () => IsResultVisible && !IsWorking);
        }
        #endregion

        #region commands
        public ICommand GetLastKnownCachedLocationCommand { get; private set; }
        public ICommand GetCurrentFineLocationCommand { get; private set; }
        public ICommand GetCurrentCoarseLocationCommand { get; private set; }
        public ICommand GetAddressFromCoordinatesCommand { get; private set; }
        #endregion

        #region delegates
        public event Action<string>? ShowAddressDialogRequested;
        #endregion

        #region methods
        private async Task GetLocation(bool fromCache, bool isFine = true)
        {
            IsWorking = true;
            (GetAddressFromCoordinatesCommand as Command)!.ChangeCanExecute();
            (LocationStatus status, Location? location) result;
            if (fromCache)
            {
                result = await gpsService.GetLastKnownCachedLocationAsync();
            }
            else
            {
                if (isFine)
                {
                    result = await gpsService.GetCurrentFineLocationAsync();
                }
                else
                {
                    result = await gpsService.GetCurrentCoarseLocationAsync();
                }
            }
            switch (result.status)
            {
                case LocationStatus.Obtained:
                    IsResultVisible = true;
                    ResultLocation = new LocationInfo
                    {
                        Latitude = result.location!.Latitude,
                        Longitude = result.location.Longitude,
                        Altitude = result.location.Altitude,
                        Accuracy = result.location.Accuracy,
                        Timestamp = result.location.Timestamp.ToLocalTime(),
                        Address = await GetAddress(result.location.Latitude, result.location.Longitude)
                    };
                    break;
                case LocationStatus.ObtainedButNull:
                    IsResultVisible = false;
                    StatusMessage = "GpsLocationStatusObtainedButNull".GetStringFromResource();
                    break;
                case LocationStatus.NotSupported:
                    IsResultVisible = false;
                    StatusMessage = "GpsLocationStatusNotSupported".GetStringFromResource();
                    break;
                case LocationStatus.NotEnabled:
                    IsResultVisible = false;
                    StatusMessage = "GpsLocationStatusNotEnabled".GetStringFromResource();
                    break;
                case LocationStatus.PermissionDenied:
                    IsResultVisible = false;
                    StatusMessage = "GpsLocationStatusPermissionDenied".GetStringFromResource();
                    break;
                case LocationStatus.OperationCancelled:
                    IsResultVisible = false;
                    StatusMessage = "GpsLocationStatusOperationCancelled".GetStringFromResource();
                    break;
                case LocationStatus.UnknownError:
                default:
                    IsResultVisible = false;
                    StatusMessage = "GpsLocationStatusUnknownError".GetStringFromResource();
                    break;
            }
            IsWorking = false;
            (GetAddressFromCoordinatesCommand as Command)!.ChangeCanExecute();
        }

        private async Task<string?> GetAddress(double latitude, double longitude)
        {
            var placemark = await gpsService.GetPlacemarkFromCoordinatesAsync(latitude, longitude);
            if (placemark is not null)
            {
                var addressLines = new List<string>();
                string streetLine = $"{placemark.Thoroughfare} {placemark.SubThoroughfare}".Trim();
                if (string.IsNullOrEmpty(streetLine) && !string.IsNullOrEmpty(placemark.FeatureName))
                {
                    streetLine = placemark.FeatureName;
                }
                if (!string.IsNullOrWhiteSpace(streetLine))
                {
                    addressLines.Add(streetLine);
                }
                if (!string.IsNullOrWhiteSpace(placemark.SubLocality) && placemark.SubLocality != placemark.Locality)
                {
                    addressLines.Add(placemark.SubLocality);
                }
                string cityLine = $"{placemark.PostalCode} {placemark.Locality}".Trim();
                if (!string.IsNullOrWhiteSpace(cityLine))
                {
                    addressLines.Add(cityLine);
                }
                string adminArea = placemark.AdminArea;
                if (!string.IsNullOrWhiteSpace(adminArea))
                {
                    addressLines.Add(adminArea);
                }
                if (!string.IsNullOrWhiteSpace(placemark.CountryName))
                {
                    addressLines.Add(placemark.CountryName);
                }

                return string.Join("\n", addressLines);
            }
            return null;
        }

        public void CancelCurrentLocationRequest() => gpsService.CancelCurrentLocationRequest();
        #endregion

        #region properties
        public LocationInfo? ResultLocation
        {
            get;
            set => SetProperty(ref field, value);
        }

        public bool IsResultVisible
        {
            get;
            set => SetProperty(ref field, value);
        }

        public string StatusMessage
        {
            get;
            set => SetProperty(ref field, value);
        } = "GpsInit".GetStringFromResource();
        #endregion
    }
}