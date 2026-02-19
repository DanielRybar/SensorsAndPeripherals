using SensorsAndPeripherals.Interfaces.Base;

namespace SensorsAndPeripherals.Interfaces.Peripherals
{
    public interface IAudioService : IPeripheralService
    {
        Task<bool> StartRecordingAsync();
        string? StopRecording();
        void PlayAudio(string filePath, Action onPlaybackFinished);
        void StopPlayback();
        void SetSpeakerMode(bool useLoudspeaker);
        TimeSpan GetAudioDuration(string filePath);
    }
}