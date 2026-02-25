using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.Models;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Platforms.Android.Services.Peripherals
{
    public class BluetoothService : AdvertiseCallback, IBluetoothService
    {
        private readonly BluetoothAdapter? adapter;
        private BluetoothLeAdvertiser? advertiser;
        private HybridDeviceReceiver? receiver;
        private bool isAdvertising;

        public BluetoothService()
        {
            var manager = global::Android.App.Application.Context.GetSystemService(Context.BluetoothService) as BluetoothManager;
            adapter = manager?.Adapter;
        }

        public bool IsSupported => adapter is not null;

        public event EventHandler<BluetoothDeviceInfo>? DeviceDiscovered;

        // via Bluetooth Low Energy
        public async Task<BluetoothResult> StartAdvertisingAsync()
        {
            if (!IsSupported)
            {
                return BluetoothResult.NotSupported;
            }
            if (adapter?.IsEnabled != true)
            {
                return BluetoothResult.NotEnabled;
            }
            if (!await CheckAdvertisingPermissionsAsync())
            {
                return BluetoothResult.PermissionDenied;
            }

            advertiser = adapter?.BluetoothLeAdvertiser;
            if (advertiser is null)
            {
                return BluetoothResult.NotSupported;
            }

            var settings = new AdvertiseSettings.Builder()?.SetAdvertiseMode(AdvertiseMode.LowLatency)?.Build();
            var data = new AdvertiseData.Builder()?.SetIncludeDeviceName(true)?.Build();
            try
            {
                advertiser?.StartAdvertising(settings, data, this);
                isAdvertising = true;
                return BluetoothResult.Success;
            }
            catch
            {
                return BluetoothResult.Error;
            }
        }

        public void StopAdvertising()
        {
            if (isAdvertising)
            {
                advertiser?.StopAdvertising(this);
                isAdvertising = false;
            }
        }

        // via Bluetooth Classic
        public async Task<BluetoothResult> StartDiscoveringAsync()
        {
            if (!IsSupported)
            {
                return BluetoothResult.NotSupported;
            }
            if (adapter?.IsEnabled != true)
            {
                return BluetoothResult.NotEnabled;
            }
            if (!await CheckDiscoveringPermissionsAsync())
            {
                return BluetoothResult.PermissionDenied;
            }

            receiver = new HybridDeviceReceiver((deviceInfo) => DeviceDiscovered?.Invoke(this, deviceInfo));
            var filter = new IntentFilter(BluetoothDevice.ActionFound);
            global::Android.App.Application.Context.RegisterReceiver(receiver, filter);
            try
            {
                var started = adapter?.StartDiscovery();
                return (started ?? false) ? BluetoothResult.Success : BluetoothResult.Error;
            }
            catch
            {
                return BluetoothResult.Error;
            }
        }

        public void StopDiscovering()
        {
            adapter?.CancelDiscovery();
            if (receiver is not null)
            {
                global::Android.App.Application.Context.UnregisterReceiver(receiver);
                receiver = null;
            }
        }

        private static async Task<bool> CheckAdvertisingPermissionsAsync()
        {
            return await CheckBluetoothPermissionAsync();
        }

        private static async Task<bool> CheckDiscoveringPermissionsAsync()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(31))
            {
                return await CheckBluetoothPermissionAsync();
            }
            else
            {
                bool btStatus = await CheckBluetoothPermissionAsync();
                bool locStatus = await CheckLocationWhenInUsePermissionAsync();
                return btStatus && locStatus;
            }
        }

        private static async Task<bool> CheckBluetoothPermissionAsync()
        {
            var btStatus = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
            if (btStatus != PermissionStatus.Granted)
            {
                btStatus = await Permissions.RequestAsync<Permissions.Bluetooth>();
            }
            return btStatus == PermissionStatus.Granted;
        }

        private static async Task<bool> CheckLocationWhenInUsePermissionAsync()
        {
            var locStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (locStatus != PermissionStatus.Granted)
            {
                locStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            return locStatus == PermissionStatus.Granted;
        }

        public override void OnStartSuccess(AdvertiseSettings? settingsInEffect) => base.OnStartSuccess(settingsInEffect);
        public override void OnStartFailure(AdvertiseFailure errorCode) => isAdvertising = false;
    }

    public class HybridDeviceReceiver(Action<BluetoothDeviceInfo> onDeviceFound) : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent?.Action == BluetoothDevice.ActionFound)
            {
                BluetoothDevice? device;
                if (OperatingSystem.IsAndroidVersionAtLeast(33))
                {
                    device = (BluetoothDevice?)intent.GetParcelableExtra(
                        BluetoothDevice.ExtraDevice,
                        Java.Lang.Class.FromType(typeof(BluetoothDevice)));
                }
                else
                {
                    device = (BluetoothDevice?)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                }
                if (device is not null)
                {
                    var info = new BluetoothDeviceInfo
                    {
                        Name = device.Name,
                        MacAddress = device.Address
                    };
                    if (string.IsNullOrEmpty(info.Name) && OperatingSystem.IsAndroidVersionAtLeast(30))
                    {
                        info.Name = device.Alias;
                    }
                    onDeviceFound?.Invoke(info);
                }
            }
        }
    }
}