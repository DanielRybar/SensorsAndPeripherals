using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.Models;
using SensorsAndPeripherals.Models.Enums;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Peripherals
{
    public class BluetoothViewModel : PeripheralViewModel<IBluetoothService>
    {
        #region constructor
        public BluetoothViewModel()
        {
            peripheralService.DeviceDiscovered += OnDeviceDiscovered;

            ToggleAdvertisingCommand = new Command(async () =>
            {
                if (IsAdvertising)
                    StopAdvertising();
                else
                    await StartAdvertisingAsync();
            }, () => IsSupported);

            ToggleDiscoveringCommand = new Command(async () =>
            {
                if (IsDiscovering)
                    StopDiscovering();
                else
                    await StartDiscoveringAsync();
            }, () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand ToggleAdvertisingCommand { get; private set; }
        public ICommand ToggleDiscoveringCommand { get; private set; }
        #endregion

        #region methods
        public void StopDiscoveringAndAdvertising()
        {
            StopAdvertising();
            StopDiscovering();
            StatusMessage = StatusMessageDiscovering = string.Empty;
        }

        private async Task StartAdvertisingAsync()
        {
            LoadingText = "StartingBLEPropagation".GetStringFromResource();
            IsWorking = true;
            await Task.Delay(500);
            var result = await peripheralService.StartAdvertisingAsync();
            StatusMessage = HandleBluetoothResult(result);
            if (result == BluetoothResult.Success)
            {
                IsAdvertising = true;
            }
            IsWorking = false;
        }

        private void StopAdvertising()
        {
            peripheralService.StopAdvertising();
            IsAdvertising = false;
        }

        private async Task StartDiscoveringAsync()
        {
            LoadingText = "ScanningStarted".GetStringFromResource();
            IsWorking = true;
            await Task.Delay(500);
            DiscoveredDevices.Clear();
            peripheralService.DeviceDiscovered += OnDeviceDiscovered;
            var result = await peripheralService.StartDiscoveringAsync();
            StatusMessageDiscovering = HandleBluetoothResult(result);
            if (result == BluetoothResult.Success)
            {
                IsDiscovering = true;
            }
            else
            {
                peripheralService.DeviceDiscovered -= OnDeviceDiscovered;
            }
            IsWorking = false;
        }

        private void StopDiscovering()
        {
            peripheralService.DeviceDiscovered -= OnDeviceDiscovered;
            peripheralService.StopDiscovering();
            IsDiscovering = false;
        }

        private void OnDeviceDiscovered(object? sender, BluetoothDeviceInfo info)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!DiscoveredDevices.Any(x => x.MacAddress == info.MacAddress))
                {
                    DiscoveredDevices.Add(info);
                }
            });
        }

        private static string HandleBluetoothResult(BluetoothResult result)
        {
            return result switch
            {
                BluetoothResult.Success => string.Empty,
                BluetoothResult.NotSupported => "BluetoothNotSupported".GetStringFromResource(),
                BluetoothResult.NotEnabled => "BluetoothNotEnabled".GetStringFromResource(),
                BluetoothResult.PermissionDenied => "BluetoothPermissionDenied".GetStringFromResource(),
                _ => "BluetoothUnknownError".GetStringFromResource(),
            };
        }
        #endregion

        #region properties
        public bool IsAdvertising
        {
            get;
            set => SetProperty(ref field, value);
        }

        public bool IsDiscovering
        {
            get;
            set => SetProperty(ref field, value);
        }

        public string LoadingText
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public string StatusMessageDiscovering
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public ObservableCollection<BluetoothDeviceInfo> DiscoveredDevices
        {
            get;
            set => SetProperty(ref field, value);
        } = [];
        #endregion
    }
}