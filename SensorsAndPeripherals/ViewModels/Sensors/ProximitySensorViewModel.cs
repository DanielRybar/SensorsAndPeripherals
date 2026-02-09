using SensorsAndPeripherals.Interfaces.Sensors;
using SensorsAndPeripherals.Models.CustomEventArgs;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class ProximitySensorViewModel : SensorViewModel<IProximitySensorService, ProximitySensorChangedEventArgs>
    {
        #region constructor
        public ProximitySensorViewModel()
        {
            DisplayText = $"{0:F2} cm";
        }
        #endregion

        #region event handlers
        protected override void OnReadingChanged(object? sender, ProximitySensorChangedEventArgs e)
        {
            float distance = e.Distance;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DisplayText = $"{distance:F2} cm";
            });
        }
        #endregion

        #region properties
        protected override SensorSpeed DefaultSpeed => SensorSpeed.UI;
        #endregion
    }
}