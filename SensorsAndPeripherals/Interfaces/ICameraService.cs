namespace SensorsAndPeripherals.Interfaces
{
    public interface ICameraService
    {
        bool IsSupported { get; }
        Task<FileResult?> TakePhotoAsync();
        Task<bool> SavePhotoToCacheAsync();
    }
}