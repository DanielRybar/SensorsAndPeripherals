using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Peripherals
{
    public class VibrationViewModel : PeripheralViewModel<IVibrationService>
    {
        #region constructor
        public VibrationViewModel()
        {
        }
        #endregion

        #region methods
        public void CancelVibration() => peripheralService.CancelVibration();
        #endregion

        #region properties

        #endregion
    }
}