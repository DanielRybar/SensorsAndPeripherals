namespace SensorsAndPeripherals.Interfaces.Peripherals
{
    public interface ICameraService
    {
        bool IsSupported { get; }
        Task<FileResult?> TakePhotoAsync();
        Task<string?> SavePhotoToCacheAsync(FileResult? photo);
    }
}