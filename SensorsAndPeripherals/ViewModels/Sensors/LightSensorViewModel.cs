using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.Models.CustomEventArgs;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class LightSensorViewModel : SensorViewModel<ILightSensorService, LightSensorChangedEventArgs>
    {
        #region variables
        private readonly double maxLogLux = 5.0; // log10(100000) = 5
        #endregion

        #region constructor
        public LightSensorViewModel()
        {
            DisplayText = $"{0:F0} lx";
        }
        #endregion

        #region event handlers
        protected override void OnReadingChanged(object? sender, LightSensorChangedEventArgs e)
        {
            float lux = e.Illuminance;
            double safeLux = Math.Max(lux, 1);
            double logVal = Math.Log10(safeLux);
            double intensity = Math.Clamp(logVal / maxLogLux, 0f, 1f);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DisplayText = $"{lux:F0} lx";
                Intensity = intensity;
            });
        }
        #endregion

        #region properties
        public double Intensity
        {
            get;
            set => SetProperty(ref field, value);
        }
        #endregion
    }
}