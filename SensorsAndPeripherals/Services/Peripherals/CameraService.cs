using SensorsAndPeripherals.Interfaces.Peripherals;

namespace SensorsAndPeripherals.Services.Peripherals
{
    public class CameraService : ICameraService
    {
        public bool IsSupported => throw new NotImplementedException();

        public Task<bool> SavePhotoToCacheAsync()
        {
            throw new NotImplementedException();
        }

        public Task<FileResult?> TakePhotoAsync()
        {
            throw new NotImplementedException();
        }
    }
}
