using SensorsAndPeripherals.Interfaces.Peripherals;

namespace SensorsAndPeripherals.Services.Peripherals
{
    public class VibrationService : IVibrationService
    {
        public bool IsSupported => Vibration.Default.IsSupported && HapticFeedback.Default.IsSupported;

        public void Vibrate(int durationInMilliseconds)
        {
            if (IsSupported)
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(durationInMilliseconds));
            }
        }

        public void CancelVibration()
        {
            if (IsSupported)
            {
                Vibration.Default.Cancel();
            }
        }

        public void PerformHapticFeedback(HapticFeedbackType feedbackType)
        {
            if (IsSupported)
            {
                HapticFeedback.Default.Perform(feedbackType);
            }
        }
    }
}