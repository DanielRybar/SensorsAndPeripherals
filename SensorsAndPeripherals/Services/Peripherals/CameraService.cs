using SensorsAndPeripherals.Interfaces.Peripherals;
using System.Diagnostics;

namespace SensorsAndPeripherals.Services.Peripherals
{
    public class CameraService : ICameraService
    {
        public bool IsSupported => MediaPicker.Default.IsCaptureSupported;

        public async Task<FileResult?> TakePhotoAsync()
        {
            if (IsSupported)
            {
                try
                {
                    return await MediaPicker.Default.CapturePhotoAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error while capturing the photo: {ex.Message}");
                }
            }
            return null;
        }

        public async Task<string?> SavePhotoToCacheAsync(FileResult? photo)
        {
            if (photo is not null)
            {
                string localPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                try
                {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream destinationStream = File.Create(localPath);
                    await sourceStream.CopyToAsync(destinationStream);
                    return localPath;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error saving photo to cache: {ex.Message}");
                    return null;
                }
            }
            return null;
        }
    }
}