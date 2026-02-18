using SensorsAndPeripherals.Interfaces.Base;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Interfaces.Peripherals
{
    public interface ICameraService : IPeripheralService
    {
        Task<(CameraResult cameraResult, FileResult? fileResult)> TakePhotoAsync();
        Task<string?> SavePhotoToCacheAsync(FileResult? photo);
    }
}