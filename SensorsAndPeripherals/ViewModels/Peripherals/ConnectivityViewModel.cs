using SensorsAndPeripherals.Constants;
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
            GetConnectionStatusCommand = new Command(GetConnectionStatus);
            TestInternetConnectionCommand = new Command(async () =>
            {
                await ExecuteSafeAsync(async () =>
                {
                    await Task.Delay(DelayConstants.MEDIUM_DELAY);
                    InternetConnectionTestResult = await peripheralService.TestInternetConnectionAsync();
                    GetConnectionStatus();
                });
            });
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
            if (!IsConnectedToNetwork)
            {
                InternetConnectionTestResult = null;
            }
            var profiles = peripheralService.GetConnectionProfiles();
            var translatedProfiles = profiles.Distinct().Select(profile => profile switch
            {
                ConnectionProfile.Cellular => "CellularCaption".SafeGetResource<string>(),
                ConnectionProfile.WiFi => "WifiCaption".SafeGetResource<string>(),
                ConnectionProfile.Unknown => "UnknownProfileCaption".SafeGetResource<string>(),
                _ => profile.ToString()
            });
            ConnectionProfiles = translatedProfiles.Any() ? string.Join("\n", translatedProfiles) : null;
        }
        #endregion

        #region properties
        public bool IsConnectedToNetwork
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