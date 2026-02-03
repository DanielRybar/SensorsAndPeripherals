using Android.Content;
using Android.Hardware;
using Android.Runtime;
using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.Models.CustomEventArgs;

namespace SensorsAndPeripherals.Platforms.Android.Services
{
    public class ProximitySensorService : Java.Lang.Object, ISensorEventListener, IProximitySensorService
    {
        private readonly SensorManager? sensorManager;
        private readonly Sensor? proximitySensor;

        public event EventHandler<ProximitySensorChangedEventArgs>? ReadingChanged;
        public bool IsMonitoring { get; private set; } = false;
        public bool IsSupported => proximitySensor is not null;

        public ProximitySensorService()
        {
            sensorManager = global::Android.App.Application.Context.GetSystemService(Context.SensorService) as SensorManager;
            proximitySensor = sensorManager?.GetDefaultSensor(SensorType.Proximity);
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
                sensorManager.RegisterListener(this, proximitySensor, delay);
                IsMonitoring = true;
            }
        }

        public void Stop()
        {
            if (IsMonitoring && sensorManager != null)
            {
                sensorManager.UnregisterListener(this, proximitySensor);
                IsMonitoring = false;
            }
        }

        public void OnSensorChanged(SensorEvent? e)
        {
            if (e is not null && e.Sensor?.Type == SensorType.Proximity && e.Values is not null)
            {
                float distance = e.Values[0];
                ReadingChanged?.Invoke(this, new ProximitySensorChangedEventArgs(distance));
            }
        }

        public void OnAccuracyChanged(Sensor? sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            return;
        }
    }
}