using CommunityToolkit.Maui.Alerts;
using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Peripherals
{
    public class VibrationViewModel : PeripheralViewModel<IVibrationService>
    {
        #region constructor
        public VibrationViewModel()
        {
            Duration = MinDuration + (MaxDuration - MinDuration) / 2;

            VibrateCommand = new Command(async () =>
            {
                if (Duration > MaxDuration || Duration < MinDuration)
                {
                    await Toast.Make($"Zadejte validní dobu vibrace mezi {MinDuration} a {MaxDuration} ms!").Show();
                    return;
                }
                CancelVibration();
                peripheralService.Vibrate(Duration);
            },
            () => IsSupported);
            PerformShortHapticFeedbackCommand = new Command(() =>
            {
                PerformHapticFeedback(HapticFeedbackType.Click);
            },
            () => IsSupported);
            PerformLongHapticFeedbackCommand = new Command(() =>
            {
                PerformHapticFeedback(HapticFeedbackType.LongPress);
            },
            () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand VibrateCommand { get; private set; }
        public ICommand PerformShortHapticFeedbackCommand { get; private set; }
        public ICommand PerformLongHapticFeedbackCommand { get; private set; }
        #endregion

        #region methods
        public void CancelVibration() => peripheralService.CancelVibration();

        private void PerformHapticFeedback(HapticFeedbackType hfType)
        {
            CancelVibration();
            peripheralService.PerformHapticFeedback(hfType);
        }
        #endregion

        #region properties
        public int Duration
        {
            get;
            set => SetProperty(ref field, value);
        }

        public int MinDuration => 1;
        public int MaxDuration => 10000;
        public string DurationText => $"{MinDuration} až {MaxDuration} ms";
        #endregion
    }
}