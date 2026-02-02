using SensorsAndPeripherals.Constants;
using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class BarometerViewModel : SensorViewModel<IBarometerService, BarometerChangedEventArgs>
    {
        #region variables
        private readonly float minPressure = 960f;
        private readonly float maxPressure = 1060f;
        #endregion

        #region constructor
        public BarometerViewModel()
        {
            DisplayText = $"{0:F2} hPa";
        }
        #endregion

        #region event handlers
        protected override void OnReadingChanged(object? sender, BarometerChangedEventArgs e)
        {
            double pressure = e.Reading.PressureInHectopascals;
            double scale = (pressure - minPressure) / (maxPressure - minPressure);
            scale = Math.Clamp(scale, 0.0, 1.0);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                BarHeightScale = scale;
                // for better readability, update text only every X ms
                if ((DateTime.Now - lastTextUpdateTime).TotalMilliseconds > SensorConstants.TEXT_VISUALIZATION_INTERVAL_MS)
                {
                    DisplayText = $"{pressure:F2} hPa";
                    lastTextUpdateTime = DateTime.Now;
                }
            });
        }
        #endregion

        #region properties
        protected override SensorSpeed DefaultSpeed => SensorSpeed.UI;

        public double BarHeightScale
        {
            get;
            set => SetProperty(ref field, value);
        } = 0.5;
        #endregion
    }
}