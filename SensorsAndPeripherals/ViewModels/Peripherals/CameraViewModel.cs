using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.Models.Enums;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Peripherals
{
    public class CameraViewModel : PeripheralViewModel<ICameraService>
    {
        #region constructor
        public CameraViewModel()
        {
            StatusMessage = "CameraInit".SafeGetResource<string>();
            TakeAndDisplayPhotoCommand = new Command(async () =>
            {
                StatusMessage = string.Empty;
                await ExecuteSafeAsync(async () =>
                {
                    var (cameraResult, fileResult) = await peripheralService.TakePhotoAsync();
                    switch (cameraResult)
                    {
                        case CameraResult.Ok:
                            if (fileResult is not null)
                            {
                                var resultPath = await peripheralService.SavePhotoToCacheAsync(fileResult);
                                if (!string.IsNullOrEmpty(resultPath))
                                {
                                    PhotoPath = resultPath;
                                }
                                else
                                {
                                    PhotoPath = null;
                                    StatusMessage = "CameraSavingError".SafeGetResource<string>();
                                }
                            }
                            break;
                        case CameraResult.PermissionDenied:
                            PhotoPath = null;
                            StatusMessage = "CameraPermissionDenied".SafeGetResource<string>();
                            break;
                        case CameraResult.Cancelled:
                            StatusMessage = "CameraInit".SafeGetResource<string>();
                            break;
                        case CameraResult.Error:
                        default:
                            PhotoPath = null;
                            StatusMessage = "CameraShootingError".SafeGetResource<string>();
                            break;
                    }
                });
            }, () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand TakeAndDisplayPhotoCommand { get; private set; }
        #endregion

        #region properties
        public string? PhotoPath
        {
            get;
            set
            {
                if (SetProperty(ref field, value))
                {
                    OnPropertyChanged(nameof(HasPhoto));
                }
            }
        } = string.Empty;

        public bool HasPhoto => !string.IsNullOrEmpty(PhotoPath);
        #endregion
    }
}