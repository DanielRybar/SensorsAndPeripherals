using SensorsAndPeripherals.Interfaces.Base;

namespace SensorsAndPeripherals.Interfaces.Peripherals
{
    public interface IVibrationService : IPeripheralService
    {
        void Vibrate(int durationInMilliseconds);
        void CancelVibration();
        void PerformHapticFeedback(HapticFeedbackType feedbackType);
    }
}