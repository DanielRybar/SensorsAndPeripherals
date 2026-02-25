using SensorsAndPeripherals.Interfaces.Base;
using SensorsAndPeripherals.Models;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Interfaces.Peripherals
{
    public interface IBluetoothService : IPeripheralService
    {
        event EventHandler<BluetoothDeviceInfo>? DeviceDiscovered;
        event EventHandler<bool>? StateChanged;
        // via Bluetooth Low Energy
        Task<BluetoothResult> StartAdvertisingAsync();
        void StopAdvertising();
        // via Bluetooth Classic
        Task<BluetoothResult> StartDiscoveringAsync();
        void StopDiscovering();
    }
}