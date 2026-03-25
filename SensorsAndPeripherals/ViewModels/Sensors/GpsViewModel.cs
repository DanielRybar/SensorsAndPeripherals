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
                ShowAddressDialogRequested?.Invoke(ResultLocation?.Address ?? "GpsAddressNotAvailable".SafeGetResource<string>());
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
            await ExecuteSafeAsync(async () =>
            {
                (GetAddressFromCoordinatesCommand as Command)!.ChangeCanExecute();
                var (status, location) = await FetchLocationAsync(fromCache, isFine);
                switch (status)
                {
                    case LocationStatus.Obtained:
                        IsResultVisible = true;
                        ResultLocation = new LocationInfo
                        {
                            Latitude = location!.Latitude,
                            Longitude = location.Longitude,
                            Altitude = location.Altitude,
                            Accuracy = location.Accuracy,
                            Timestamp = location.Timestamp.ToLocalTime(),
                            Address = await GetAddress(location.Latitude, location.Longitude)
                        };
                        break;
                    case LocationStatus.ObtainedButNull:
                        IsResultVisible = false;
                        StatusMessage = "GpsLocationStatusObtainedButNull".SafeGetResource<string>();
                        break;
                    case LocationStatus.NotSupported:
                        IsResultVisible = false;
                        StatusMessage = "GpsLocationStatusNotSupported".SafeGetResource<string>();
                        break;
                    case LocationStatus.NotEnabled:
                        IsResultVisible = false;
                        StatusMessage = "GpsLocationStatusNotEnabled".SafeGetResource<string>();
                        break;
                    case LocationStatus.PermissionDenied:
                        IsResultVisible = false;
                        StatusMessage = "GpsLocationStatusPermissionDenied".SafeGetResource<string>();
                        break;
                    case LocationStatus.OperationCancelled:
                        IsResultVisible = false;
                        StatusMessage = "GpsLocationStatusOperationCancelled".SafeGetResource<string>();
                        break;
                    case LocationStatus.UnknownError:
                    default:
                        IsResultVisible = false;
                        StatusMessage = "GpsLocationStatusUnknownError".SafeGetResource<string>();
                        break;
                }
            });
            (GetAddressFromCoordinatesCommand as Command)!.ChangeCanExecute();
        }

        private Task<(LocationStatus status, Location? location)> FetchLocationAsync(bool fromCache, bool isFine)
        {
            if (fromCache)
            {
                return gpsService.GetLastKnownCachedLocationAsync();
            }
            return isFine
                ? gpsService.GetCurrentFineLocationAsync()
                : gpsService.GetCurrentCoarseLocationAsync();
        }

        private async Task<string?> GetAddress(double latitude, double longitude)
        {
            var placemark = await gpsService.GetPlacemarkFromCoordinatesAsync(latitude, longitude);
            if (placemark is null)
            {
                return null;
            }
            return BuildAddressString(placemark);
        }

        private static string BuildAddressString(Placemark placemark)
        {
            var lines = new List<string>();
            AddStreetLine(placemark, lines);
            AddSubLocalityLine(placemark, lines);
            AddCityLine(placemark, lines);
            AddAdminAreaLine(placemark, lines);
            AddCountryLine(placemark, lines);
            return string.Join("\n", lines);
        }

        private static void AddStreetLine(Placemark placemark, List<string> lines)
        {
            string street = $"{placemark.Thoroughfare} {placemark.SubThoroughfare}".Trim();
            if (string.IsNullOrEmpty(street) && !string.IsNullOrEmpty(placemark.FeatureName))
            {
                street = placemark.FeatureName;
            }
            if (!string.IsNullOrWhiteSpace(street))
            {
                lines.Add(street);
            }
        }

        private static void AddSubLocalityLine(Placemark placemark, List<string> lines)
        {
            if (!string.IsNullOrWhiteSpace(placemark.SubLocality) && placemark.SubLocality != placemark.Locality)
            {
                lines.Add(placemark.SubLocality);
            }
        }

        private static void AddCityLine(Placemark placemark, List<string> lines)
        {
            string city = $"{placemark.PostalCode} {placemark.Locality}".Trim();
            if (!string.IsNullOrWhiteSpace(city))
            {
                lines.Add(city);
            }
        }

        private static void AddAdminAreaLine(Placemark placemark, List<string> lines)
        {
            if (!string.IsNullOrWhiteSpace(placemark.AdminArea))
            {
                lines.Add(placemark.AdminArea);
            }
        }

        private static void AddCountryLine(Placemark placemark, List<string> lines)
        {
            if (!string.IsNullOrWhiteSpace(placemark.CountryName))
            {
                lines.Add(placemark.CountryName);
            }
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
        } = "GpsInit".SafeGetResource<string>();
        #endregion
    }
}