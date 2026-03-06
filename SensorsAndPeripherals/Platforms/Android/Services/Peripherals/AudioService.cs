using Android.Content;
using Android.Media;
using Android.OS;
using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.Models.Enums;
using Debug = System.Diagnostics.Debug;

namespace SensorsAndPeripherals.Platforms.Android.Services.Peripherals
{
    public class AudioService : IAudioService
    {
        private MediaRecorder? mediaRecorder;
        private MediaPlayer? mediaPlayer;
        private string? currentFilePath;
        private readonly AudioManager? audioManager;

        public AudioService()
        {
            audioManager = global::Android.App.Application.Context.GetSystemService(Context.AudioService) as AudioManager;
        }

        public bool IsSupported => CheckAvailability();

        public async Task<MicrophoneResult> StartRecordingAsync()
        {
            if (!IsSupported)
            {
                return MicrophoneResult.NotSupported;
            }
            var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Microphone>();
                if (status != PermissionStatus.Granted)
                {
                    return MicrophoneResult.PermissionDenied;
                }
            }
            try
            {
                string fileName = $"record_{DateTime.Now:yyyyMMddHHmmss}.3gp";
                currentFilePath = Path.Combine(FileSystem.CacheDirectory, fileName);

                mediaRecorder = CreateMediaRecorder();
                mediaRecorder.SetAudioSource(AudioSource.Mic);
                mediaRecorder.SetOutputFormat(OutputFormat.ThreeGpp);
                mediaRecorder.SetAudioEncoder(AudioEncoder.AmrNb);
                mediaRecorder.SetOutputFile(currentFilePath);
                mediaRecorder.Prepare();
                mediaRecorder.Start();
                return MicrophoneResult.Ok;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while recording: {ex.Message}");
                return MicrophoneResult.Error;
            }
        }

        public string? StopRecording()
        {
            try
            {
                if (mediaRecorder != null)
                {
                    mediaRecorder.Stop();
                    mediaRecorder.Release();
                    mediaRecorder = null;
                }
                var path = currentFilePath;
                currentFilePath = null;
                return path;
            }
            catch
            {
                currentFilePath = null;
                return null;
            }
        }

        public void PlayAudio(string filePath, Action onPlaybackFinished)
        {
            if (!IsSupported)
            {
                return;
            }
            StopPlayback();
            try
            {
                mediaPlayer = new MediaPlayer();
                mediaPlayer.SetDataSource(filePath);
                mediaPlayer.SetAudioAttributes(new AudioAttributes.Builder()
                    .SetUsage(AudioUsageKind.Media)!
                    .SetContentType(AudioContentType.Speech)!
                    .Build());

                mediaPlayer.Prepare();
                mediaPlayer.Start();
                mediaPlayer.Completion += (s, e) =>
                {
                    StopPlayback();
                    onPlaybackFinished?.Invoke();
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while playing the audio: {ex.Message}");
                onPlaybackFinished?.Invoke();
            }
        }

        public void StopPlayback()
        {
            if (mediaPlayer is not null)
            {
                if (mediaPlayer.IsPlaying)
                {
                    mediaPlayer.Stop();
                }
                mediaPlayer.Release();
                mediaPlayer = null;
            }
        }

        public void SetSpeakerMode(bool useLoudspeaker)
        {
            if (mediaPlayer is null)
            {
                return;
            }
            var devices = audioManager?.GetDevices(GetDevicesTargets.Outputs) ?? [];
            AudioDeviceInfo? targetDevice = null;
            foreach (var device in devices)
            {
                if (useLoudspeaker && device.Type == AudioDeviceType.BuiltinSpeaker)
                {
                    targetDevice = device;
                    break;
                }
                if (!useLoudspeaker && device.Type == AudioDeviceType.BuiltinEarpiece)
                {
                    targetDevice = device;
                    break;
                }
            }
            if (targetDevice != null)
            {
                mediaPlayer.SetPreferredDevice(targetDevice);
            }
        }

        public TimeSpan GetAudioDuration(string filePath)
        {
            try
            {
                using var retriever = new MediaMetadataRetriever();
                retriever.SetDataSource(filePath);
                string? time = retriever.ExtractMetadata(MetadataKey.Duration);
                if (long.TryParse(time, out long timeMs))
                {
                    return TimeSpan.FromMilliseconds(timeMs);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while reading audio length: {ex.Message}");
            }
            return TimeSpan.Zero;
        }

        private static MediaRecorder CreateMediaRecorder()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
#pragma warning disable CA1416 // Compatibility verified by condition
                return new MediaRecorder(global::Android.App.Application.Context);
#pragma warning restore CA1416 // Compatibility verified by condition

#pragma warning disable CA1422 // Compatibility verified by condition
            return new MediaRecorder();
#pragma warning restore CA1422 // Compatibility verified by condition
        }

        private bool CheckAvailability()
        {
            var context = global::Android.App.Application.Context;
            if (audioManager is null)
            {
                return false;
            }
            bool? hasMic = context.PackageManager?.HasSystemFeature(global::Android.Content.PM.PackageManager.FeatureMicrophone);
            return hasMic ?? false;
        }
    }
}