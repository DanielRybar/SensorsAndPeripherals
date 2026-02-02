using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.Models.CustomEventArgs;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class LightSensorViewModel : SensorViewModel<ILightSensorService, LightSensorChangedEventArgs>
    {
        #region constructor
        public LightSensorViewModel()
        {
            DisplayText = $"{0:F2} lx";
        }
        #endregion

        #region event handlers
        protected override void OnReadingChanged(object? sender, LightSensorChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DisplayText = $"{e.Illuminance:F2} lx";
            });
        }
        #endregion
    }
}