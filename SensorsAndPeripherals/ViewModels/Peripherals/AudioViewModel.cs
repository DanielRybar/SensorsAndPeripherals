using SensorsAndPeripherals.Constants;
using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.Models.Enums;
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
        private readonly TimeSpan maxRecordingDuration = TimeSpan.FromMinutes(5);
        private TimeSpan totalDuration;
        #endregion

        #region constructor
        public AudioViewModel()
        {
            timer.Interval = TimeSpan.FromMilliseconds(PeripheralConstants.TEXT_VISUALIZATION_INTERVAL_MS);
            timer.Tick += (s, e) => UpdateTimeDisplay();
            ToggleRecordCommand = new Command(async () => await ToggleRecordingAsync(), () => IsSupported && !IsPlaying);
            TogglePlayCommand = new Command(TogglePlayback, () => IsSupported && !IsRecording && HasRecordedFile);
            ToggleSpeakerCommand = new Command(ToggleSpeakerMode, () => IsSupported && HasRecordedFile);
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
            }
            else
            {
                TimeDisplay = "00:00";
            }
        }

        private async Task StartRecordingAsync()
        {
            IsWorking = true;
            await Task.Delay(500);
            RecordedFilePath = null;
            var started = await peripheralService.StartRecordingAsync();
            switch (started)
            {
                case MicrophoneResult.Ok:
                    IsRecording = true;
                    stopwatch.Restart();
                    timer.Start();
                    break;
                case MicrophoneResult.PermissionDenied:
                    ErrorMessage = "MicrophonePermissionDenied".GetStringFromResource();
                    break;
                case MicrophoneResult.NotSupported:
                    ErrorMessage = "AudioNotSupported".GetStringFromResource();
                    break;
                case MicrophoneResult.Error:
                default:
                    ErrorMessage = "RecordingError".GetStringFromResource();
                    break;
            }
            IsWorking = false;
        }

        private void UpdateTimeDisplay()
        {
            if (IsRecording)
            {
                if (stopwatch.Elapsed >= maxRecordingDuration)
                {
                    StopRecording();
                    ErrorMessage = "RecordingTooLong".GetStringFromResource();
                    return;
                }
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
            ErrorMessage = string.Empty;
            if (IsRecording)
            {
                StopRecording();
            }
            else
            {
                await StartRecordingAsync();
            }
        }

        private void TogglePlayback()
        {
            ErrorMessage = string.Empty;
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
            ErrorMessage = string.Empty;
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

        private void RefreshCommands()
        {
            (ToggleRecordCommand as Command)!.ChangeCanExecute();
            (TogglePlayCommand as Command)!.ChangeCanExecute();
            (ToggleSpeakerCommand as Command)!.ChangeCanExecute();
        }
        #endregion

        #region properties
        public bool IsRecording
        {
            get;
            set
            {
                if (SetProperty(ref field, value))
                {
                    RefreshCommands();
                }
            }
        }

        public bool IsPlaying
        {
            get;
            set
            {
                if (SetProperty(ref field, value))
                {
                    RefreshCommands();
                }
            }
        }

        public string? RecordedFilePath
        {
            get;
            set
            {
                if (SetProperty(ref field, value))
                {
                    OnPropertyChanged(nameof(HasRecordedFile));
                    RefreshCommands();
                }
            }
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

        public string ErrorMessage
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public bool HasRecordedFile => !string.IsNullOrEmpty(RecordedFilePath);
        #endregion
    }
}