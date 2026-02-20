using SensorsAndPeripherals.Interfaces.Base;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Interfaces.Peripherals
{
    public interface IAudioService : IPeripheralService
    {
        Task<MicrophoneResult> StartRecordingAsync();
        string? StopRecording();
        void PlayAudio(string filePath, Action onPlaybackFinished);
        void StopPlayback();
        void SetSpeakerMode(bool useLoudspeaker);
        TimeSpan GetAudioDuration(string filePath);
    }
}