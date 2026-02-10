using SensorsAndPeripherals.Interfaces.Base;

namespace SensorsAndPeripherals.Interfaces.Peripherals
{
    public interface ICameraService : IPeripheralService
    {
        Task<FileResult?> TakePhotoAsync();
        Task<string?> SavePhotoToCacheAsync(FileResult? photo);
    }
}