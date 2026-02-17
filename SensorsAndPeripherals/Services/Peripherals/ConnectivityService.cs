using SensorsAndPeripherals.Constants;
using SensorsAndPeripherals.Interfaces.Peripherals;

namespace SensorsAndPeripherals.Services.Peripherals
{
    public class ConnectivityService : IConnectivityService
    {
        private static readonly HttpClient httpClient = new() { Timeout = TimeSpan.FromSeconds(10) };
        private readonly NetworkAccess[] possibleSuccessfulConnections =
        [
            NetworkAccess.Internet,
            NetworkAccess.ConstrainedInternet,
            NetworkAccess.Local
        ];

        public bool IsSupported => true; // Network stack is core of modern OS, so we can assume it's always supported

        public bool IsConnectedToNetwork => possibleSuccessfulConnections.Contains(Connectivity.Current.NetworkAccess);

        public IEnumerable<ConnectionProfile> GetConnectionProfiles()
        {
            return Connectivity.Current.ConnectionProfiles;
        }

        public async Task<bool> TestInternetConnectionAsync()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return false;
            }
            try
            {
                var response = await httpClient.GetAsync(PeripheralConstants.TEST_INTERNET_CONNECTION_URL);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}