using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.Models;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;
using LocationStatus = SensorsAndPeripherals.Models.Enums.LocationStatus;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class GpsViewModel : BaseViewModel
    {
        #region services
        private readonly IGeolocationService geolocationService = DependencyService.Get<IGeolocationService>();
        #endregion

        #region constructor
        public GpsViewModel()
        {
            GetLastKnownCachedLocationCommand = new Command(async () =>
            {
                await HandleLocation(fromCache: true);
            });
            GetCurrentFineLocationCommand = new Command(async () =>
            {
                await HandleLocation(fromCache: false, isFine: true);
            });
            GetCurrentCoarseLocationCommand = new Command(async () =>
            {
                await HandleLocation(fromCache: false, isFine: false);
            });
        }
        #endregion

        #region commands
        public ICommand GetLastKnownCachedLocationCommand { get; private set; }
        public ICommand GetCurrentFineLocationCommand { get; private set; }
        public ICommand GetCurrentCoarseLocationCommand { get; private set; }
        #endregion

        #region methods
        private async Task HandleLocation(bool fromCache, bool isFine = true)
        {
            IsWorking = true;
            (LocationStatus status, Location? location) result;
            if (fromCache)
            {
                result = await geolocationService.GetLastKnownCachedLocation();
            }
            else
            {
                if (isFine)
                {
                    result = await geolocationService.GetCurrentFineLocation();
                }
                else
                {
                    result = await geolocationService.GetCurrentCoarseLocation();
                }
            }
            switch (result.status)
            {
                case LocationStatus.Obtained:
                    IsResultVisible = true;
                    ResultLocation = new LocationInfo
                    {
                        Latitude = result.location!.Latitude,
                        Longitude = result.location!.Longitude,
                        Altitude = result.location!.Altitude,
                        Accuracy = result.location!.Accuracy,
                        Timestamp = result.location!.Timestamp.ToLocalTime()
                    };
                    break;
                case LocationStatus.ObtainedButNull:
                    IsResultVisible = false;
                    StatusMessage = "Polohu se nepodařilo zjistit (žádná data nejsou k dispozici).";
                    break;
                case LocationStatus.NotSupported:
                    IsResultVisible = false;
                    StatusMessage = "Toto zařízení nepodporuje zjišťování polohy.";
                    break;
                case LocationStatus.NotEnabled:
                    IsResultVisible = false;
                    StatusMessage = "GPS je vypnutá. Prosím, zapněte ji v nastavení telefonu.";
                    break;
                case LocationStatus.PermissionDenied:
                    IsResultVisible = false;
                    StatusMessage = "Aplikace nemá oprávnění k poloze. Povolte přístup v nastavení aplikace.";
                    break;
                case LocationStatus.OperationCancelled:
                    IsResultVisible = false;
                    StatusMessage = "Získávání polohy bylo zrušeno.";
                    break;
                case LocationStatus.UnknownError:
                default:
                    IsResultVisible = false;
                    StatusMessage = "Došlo k neznámé chybě při zjišťování polohy.";
                    break;
            }
            IsWorking = false;
        }

        public void CancelCurrentLocationRequest() => geolocationService.CancelCurrentLocationRequest();
        #endregion

        #region properties
        public LocationInfo? ResultLocation
        {
            get;
            set => SetProperty(ref field, value);
        } = new();

        public bool IsResultVisible
        {
            get;
            set => SetProperty(ref field, value);
        } = true;

        public string StatusMessage
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;
        #endregion
    }
}