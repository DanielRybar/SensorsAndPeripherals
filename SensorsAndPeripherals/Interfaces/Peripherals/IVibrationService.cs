namespace SensorsAndPeripherals.Interfaces.Peripherals
{
    public interface IVibrationService
    {
        bool IsSupported { get; }
        void Vibrate(int durationInMilliseconds);
        void CancelVibration();
        void PerformHapticFeedback(HapticFeedbackType feedbackType);
    }
}