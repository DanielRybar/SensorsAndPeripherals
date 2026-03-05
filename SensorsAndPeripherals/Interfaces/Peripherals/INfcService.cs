using SensorsAndPeripherals.Interfaces.Base;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Interfaces.Peripherals
{
    public interface INfcService : IPeripheralService
    {
        Task<(NfcStatus status, string? content)> ScanAsync();
        Task<NfcStatus> WriteAsync(string content);
        void CancelCurrentRequests();
    }
}