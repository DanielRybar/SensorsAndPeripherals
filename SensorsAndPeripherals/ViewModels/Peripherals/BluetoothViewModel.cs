using SensorsAndPeripherals.Constants;
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
            ToggleAdvertisingCommand = new Command(async () =>
            {
                if (IsAdvertising)
                    StopAdvertising();
                else
                    await StartAdvertisingAsync();
            },
            () => IsSupported);
            ToggleDiscoveringCommand = new Command(async () =>
            {
                if (IsDiscovering)
                    StopDiscovering();
                else
                    await StartDiscoveringAsync();
            },
            () => IsSupported);
            GetAdapterInfoCommand = new Command(async () =>
            {
                ShowAdapterInfoDialogRequested?.Invoke(await peripheralService.GetAdapterName() ?? "BluetoothUnknownDevice".SafeGetResource<string>());
            },
            () => IsSupported && !IsWorking);
        }
        #endregion

        #region commands
        public ICommand ToggleAdvertisingCommand { get; private set; }
        public ICommand ToggleDiscoveringCommand { get; private set; }
        public ICommand GetAdapterInfoCommand { get; private set; }
        #endregion

        #region delegates
        public event Action<string>? ShowAdapterInfoDialogRequested;
        #endregion

        #region methods
        public void Initialize()
        {
            HandleBluetoothEvents(register: false);
            HandleBluetoothEvents(register: true);
        }

        public void CleanUp()
        {
            StopAdvertising();
            StopDiscovering();
            HandleBluetoothEvents(register: false);
            StatusMessage = StatusMessageDiscovering = string.Empty;
            DiscoveredDevices.Clear();
        }

        private async Task StartAdvertisingAsync()
        {
            LoadingText = "StartingBLEPropagation".SafeGetResource<string>();
            IsWorking = true;
            await Task.Delay(DelayConstants.MEDIUM_DELAY);
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
            LoadingText = "ScanningStarted".SafeGetResource<string>();
            IsWorking = true;
            await Task.Delay(DelayConstants.MEDIUM_DELAY);
            DiscoveredDevices.Clear();
            var result = await peripheralService.StartDiscoveringAsync();
            StatusMessageDiscovering = HandleBluetoothResult(result);
            if (result == BluetoothResult.Success)
            {
                IsDiscovering = true;
            }
            IsWorking = false;
        }

        private void StopDiscovering()
        {
            peripheralService.StopDiscovering();
            IsDiscovering = false;
        }

        private void OnDeviceDiscovered(object? sender, BluetoothDeviceInfo info)
        {
            if (!IsDiscovering)
            {
                return;
            }
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var device = DiscoveredDevices.FirstOrDefault(x => x.MacAddress == info.MacAddress);
                if (device is not null)
                {
                    int index = DiscoveredDevices.IndexOf(device);
                    DiscoveredDevices[index] = info;
                }
                else
                {
                    DiscoveredDevices.Add(info);
                }
            });
        }

        private void OnBluetoothStateChanged(object? sender, bool isOn)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (isOn)
                {
                    StatusMessage = StatusMessageDiscovering = string.Empty;
                    return;
                }
                if (IsAdvertising)
                {
                    StopAdvertising();
                    StatusMessage = "BluetoothExternalTurnOff".SafeGetResource<string>();
                }
                if (IsDiscovering)
                {
                    StopDiscovering();
                    StatusMessageDiscovering = "BluetoothExternalTurnOff".SafeGetResource<string>();
                }
            });
        }

        private void HandleBluetoothEvents(bool register)
        {
            if (register)
            {
                peripheralService.DeviceDiscovered += OnDeviceDiscovered;
                peripheralService.StateChanged += OnBluetoothStateChanged;
            }
            else
            {
                peripheralService.DeviceDiscovered -= OnDeviceDiscovered;
                peripheralService.StateChanged -= OnBluetoothStateChanged;
            }
        }

        private static string HandleBluetoothResult(BluetoothResult result)
        {
            return result switch
            {
                BluetoothResult.Success => string.Empty,
                BluetoothResult.NotSupported => "BluetoothNotSupported".SafeGetResource<string>(),
                BluetoothResult.NotEnabled => "BluetoothNotEnabled".SafeGetResource<string>(),
                BluetoothResult.PermissionDenied => "BluetoothPermissionDenied".SafeGetResource<string>(),
                _ => "BluetoothUnknownError".SafeGetResource<string>(),
            };
        }
        #endregion

        #region properties
        public override bool IsWorking
        {
            get;
            set
            {
                if (SetProperty(ref field, value))
                {
                    (GetAdapterInfoCommand as Command)!.ChangeCanExecute();
                }
            }
        }

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