using SensorsAndPeripherals.Interfaces.Base;

namespace SensorsAndPeripherals.Interfaces.Peripherals
{
    public interface IConnectivityService : IPeripheralService
    {
        bool IsConnectedToNetwork { get; }
        IEnumerable<ConnectionProfile> GetConnectionProfiles();
        Task<bool> TestInternetConnectionAsync();
    }
}