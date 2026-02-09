using Android.Content;
using Android.Hardware;
using Android.Runtime;
using SensorsAndPeripherals.Interfaces.Sensors;
using SensorsAndPeripherals.Models.CustomEventArgs;

namespace SensorsAndPeripherals.Platforms.Android.Services.Sensors
{
    public class LightSensorService : Java.Lang.Object, ISensorEventListener, ILightSensorService
    {
        private readonly SensorManager? sensorManager;
        private readonly Sensor? lightSensor;

        public event EventHandler<LightSensorChangedEventArgs>? ReadingChanged;
        public bool IsMonitoring { get; private set; } = false;
        public bool IsSupported => lightSensor is not null;

        public LightSensorService()
        {
            sensorManager = global::Android.App.Application.Context.GetSystemService(Context.SensorService) as SensorManager;
            lightSensor = sensorManager?.GetDefaultSensor(SensorType.Light);
        }

        public void Start(SensorSpeed speed)
        {
            if (IsSupported && !IsMonitoring && sensorManager is not null)
            {
                var delay = speed switch
                {
                    SensorSpeed.Fastest => SensorDelay.Fastest,
                    SensorSpeed.Game => SensorDelay.Game,
                    SensorSpeed.UI => SensorDelay.Ui,
                    _ => SensorDelay.Normal
                };
                sensorManager.RegisterListener(this, lightSensor, delay);
                IsMonitoring = true;
            }
        }

        public void Stop()
        {
            if (IsMonitoring && sensorManager != null)
            {
                sensorManager.UnregisterListener(this, lightSensor);
                IsMonitoring = false;
            }
        }

        public void OnSensorChanged(SensorEvent? e)
        {
            if (e is not null && e.Sensor?.Type == SensorType.Light && e.Values is not null)
            {
                float lux = e.Values[0];
                ReadingChanged?.Invoke(this, new LightSensorChangedEventArgs(lux));
            }
        }

        public void OnAccuracyChanged(Sensor? sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            return;
        }
    }
}