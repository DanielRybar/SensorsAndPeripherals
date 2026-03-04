using SensorsAndPeripherals.Interfaces.Base;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Interfaces.Peripherals
{
    public interface INfcService : IPeripheralService
    {
        Task<(NfcStatus status, string? content)> ScanAsync();
        Task<(NfcStatus status, bool isSuccess)> WriteAsync(string content);
        void CancelCurrentRequest();
    }
}