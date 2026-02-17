using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Peripherals
{
    public class ConnectivityViewModel : PeripheralViewModel<IConnectivityService>
    {
        #region constructor
        public ConnectivityViewModel()
        {
            GetConnectionStatusCommand = new Command(GetConnectionStatus, () => IsSupported);
            TestInternetConnectionCommand = new Command(async () =>
            {
                IsWorking = true;
                await Task.Delay(500);
                InternetConnectionTestResult = await peripheralService.TestInternetConnectionAsync();
                GetConnectionStatus();
                IsWorking = false;
            }, () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand GetConnectionStatusCommand { get; private set; }
        public ICommand TestInternetConnectionCommand { get; private set; }
        #endregion

        #region methods
        private void GetConnectionStatus()
        {
            IsConnectedToNetwork = peripheralService.IsConnectedToNetwork;
            var profiles = peripheralService.GetConnectionProfiles();
            var translatedProfiles = profiles.Select(profile => profile switch
            {
                ConnectionProfile.Cellular => "CellularCaption".GetStringFromResource(),
                ConnectionProfile.WiFi => "WifiCaption".GetStringFromResource(),
                ConnectionProfile.Unknown => "UnknownProfileCaption".GetStringFromResource(),
                _ => profile.ToString()
            });
            if (translatedProfiles.Any())
            {
                ConnectionProfiles = string.Join(", ", translatedProfiles);
            }
            else
            {
                ConnectionProfiles = null;
            }
        }
        #endregion

        #region properties
        public bool? IsConnectedToNetwork
        {
            get;
            set => SetProperty(ref field, value);
        }

        public bool? InternetConnectionTestResult
        {
            get;
            set => SetProperty(ref field, value);
        }

        public string? ConnectionProfiles
        {
            get;
            set => SetProperty(ref field, value);
        } = null;
        #endregion
    }
}