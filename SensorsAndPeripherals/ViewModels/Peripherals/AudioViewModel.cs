using SensorsAndPeripherals.Constants;
using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Diagnostics;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Peripherals
{
    public class AudioViewModel : PeripheralViewModel<IAudioService>
    {
        #region variables
        private readonly IDispatcherTimer timer = App.Current!.Dispatcher.CreateTimer();
        private readonly Stopwatch stopwatch = new();
        private TimeSpan totalDuration;
        #endregion

        #region constructor
        public AudioViewModel()
        {
            timer.Interval = TimeSpan.FromMilliseconds(PeripheralConstants.TEXT_VISUALIZATION_INTERVAL_MS);
            timer.Tick += (s, e) => UpdateTimeDisplay();
            ToggleRecordCommand = new Command(async () => await ToggleRecordingAsync());
            TogglePlayCommand = new Command(TogglePlaybackAsync);
            ToggleSpeakerCommand = new Command(ToggleSpeakerMode);
        }
        #endregion

        #region commands
        public ICommand ToggleRecordCommand { get; private set; }
        public ICommand TogglePlayCommand { get; private set; }
        public ICommand ToggleSpeakerCommand { get; private set; }
        #endregion

        #region methods
        public void StopRecording()
        {
            RecordedFilePath = peripheralService.StopRecording();
            IsRecording = false;
            stopwatch.Stop();
            timer.Stop();
            if (HasRecordedFile)
            {
                totalDuration = peripheralService.GetAudioDuration(RecordedFilePath!);
                TimeDisplay = totalDuration.ToString(@"mm\:ss");
                OnPropertyChanged(nameof(HasRecordedFile));
            }
            else
            {
                TimeDisplay = "00:00";
            }
        }

        private async Task StartRecording()
        {
            RecordedFilePath = null;
            OnPropertyChanged(nameof(HasRecordedFile));
            bool started = await peripheralService.StartRecordingAsync();
            if (started)
            {
                IsRecording = true;
                stopwatch.Restart();
                timer.Start();
            }
            else
            {
                StatusMessage = "RecordingError".GetStringFromResource();
            }
        }

        private void UpdateTimeDisplay()
        {
            if (IsRecording)
            {
                TimeDisplay = stopwatch.Elapsed.ToString(@"mm\:ss");
            }
            else if (IsPlaying)
            {
                TimeDisplay = $"{stopwatch.Elapsed:mm\\:ss} / {totalDuration:mm\\:ss}";
                if (stopwatch.Elapsed > totalDuration)
                {
                    TimeDisplay = $"{totalDuration:mm\\:ss} / {totalDuration:mm\\:ss}";
                }
            }
        }

        private async Task ToggleRecordingAsync()
        {
            if (IsRecording)
            {
                StopRecording();
            }
            else
            {
                await StartRecording();
            }
        }

        private async void TogglePlaybackAsync()
        {
            if (!HasRecordedFile) return;
            if (IsPlaying)
            {
                peripheralService.StopPlayback();
                ResetPlaybackUI();
            }
            else
            {
                IsPlaying = true;
                stopwatch.Restart();
                timer.Start();
                peripheralService.SetSpeakerMode(IsLoudspeaker);
                peripheralService.PlayAudio(RecordedFilePath!, () =>
                {
                    MainThread.BeginInvokeOnMainThread(ResetPlaybackUI);
                });
            }

        }

        private void ToggleSpeakerMode()
        {
            IsLoudspeaker = !IsLoudspeaker;
            if (IsPlaying)
            {
                peripheralService.SetSpeakerMode(IsLoudspeaker);
            }
        }

        private void ResetPlaybackUI()
        {
            IsPlaying = false;
            stopwatch.Stop();
            timer.Stop();
            TimeDisplay = totalDuration.ToString(@"mm\:ss");
        }
        #endregion

        #region properties
        public bool IsRecording 
        { 
            get; 
            set => SetProperty(ref field, value); 
        }

        public bool IsPlaying 
        { 
            get; 
            set => SetProperty(ref field, value);
        }

        public string? RecordedFilePath
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public string TimeDisplay 
        { 
            get; 
            set => SetProperty(ref field, value); 
        } = "00:00";

        public bool IsLoudspeaker
        {
            get;
            set => SetProperty(ref field, value);
        } = true;

        public string StatusMessage
        {
            get => field;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public bool HasRecordedFile => !string.IsNullOrEmpty(RecordedFilePath);
        #endregion
    }
}