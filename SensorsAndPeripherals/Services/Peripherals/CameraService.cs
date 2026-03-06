using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.Models.Enums;
using System.Diagnostics;

namespace SensorsAndPeripherals.Services.Peripherals
{
    public class CameraService : ICameraService
    {
        public bool IsSupported => MediaPicker.Default.IsCaptureSupported;

        public async Task<(CameraResult cameraResult, FileResult? fileResult)> TakePhotoAsync()
        {
            if (!IsSupported)
            {
                return (CameraResult.Error, null);
            }
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    return (CameraResult.PermissionDenied, null);
                }
            }

            try
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo is null)
                {
                    return (CameraResult.Cancelled, null);
                }
                return (CameraResult.Ok, photo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while capturing the photo: {ex.Message}");
                return (CameraResult.Error, null);
            }
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