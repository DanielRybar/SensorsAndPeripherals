using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.Services;

namespace SensorsAndPeripherals
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                    fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Services
            DependencyService.Register<IAccelerometerService, AccelerometerService>();
            DependencyService.Register<ISensorListService, Platforms.Android.Services.SensorListService>();

            return builder.Build();
        }
    }
}