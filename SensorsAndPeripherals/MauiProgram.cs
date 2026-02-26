using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SensorsAndPeripherals.Controls;
using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.Interfaces.Sensors;
using SensorsAndPeripherals.Services.Peripherals;
using SensorsAndPeripherals.Services.Sensors;

namespace SensorsAndPeripherals
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit(static options =>
                {
                    options.SetPopupDefaults(new DefaultPopupSettings
                    {
                        CanBeDismissedByTappingOutsideOfPopup = false,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        Margin = 0,
                        Padding = 0
                    });
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                    fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler<ExtendedEntry, Platforms.Android.Handlers.ExtendedEntryHandler>();
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Services
            DependencyService.Register<IAccelerometerService, AccelerometerService>();
            DependencyService.Register<IGyroscopeService, GyroscopeService>();
            DependencyService.Register<IMagnetometerService, MagnetometerService>();
            DependencyService.Register<IRotationSensorService, RotationSensorService>();
            DependencyService.Register<IBarometerService, BarometerService>();
            DependencyService.Register<IGpsService, GpsService>();
            DependencyService.Register<IBiometricService, BiometricService>();
            DependencyService.Register<ICameraService, CameraService>();
            DependencyService.Register<IVibrationService, VibrationService>();
            DependencyService.Register<IConnectivityService, ConnectivityService>();
            DependencyService.Register<ILightSensorService, Platforms.Android.Services.Sensors.LightSensorService>();
            DependencyService.Register<IProximitySensorService, Platforms.Android.Services.Sensors.ProximitySensorService>();
            DependencyService.Register<IAudioService, Platforms.Android.Services.Peripherals.AudioService>();
            DependencyService.Register<IBluetoothService, Platforms.Android.Services.Peripherals.BluetoothService>();
            DependencyService.Register<ISensorListService, Platforms.Android.Services.SensorListService>();

            return builder.Build();
        }
    }
}