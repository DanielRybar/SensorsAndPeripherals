using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.ViewModels.Abstract;

namespace SensorsAndPeripherals.ViewModels.Peripherals
{
    public class NfcViewModel : PeripheralViewModel<INfcService>
    {
        public NfcViewModel()
        {
        }
    }
}