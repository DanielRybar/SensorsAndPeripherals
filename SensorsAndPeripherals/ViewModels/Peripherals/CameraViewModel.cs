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
            TakeAndDisplayPhotoCommand = new Command(async () =>
            {
                IsWorking = true;
                StatusMessage = string.Empty;
                try
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
                                    StatusMessage = "CameraSavingError".GetStringFromResource();
                                }
                            }
                            break;
                        case CameraResult.PermissionDenied:
                            PhotoPath = null;
                            StatusMessage = "CameraPermissionDenied".GetStringFromResource();
                            break;
                        case CameraResult.Cancelled:
                            StatusMessage = "CameraInit".GetStringFromResource();
                            break;
                        case CameraResult.Error:
                        default:
                            PhotoPath = null;
                            StatusMessage = "CameraShootingError".GetStringFromResource();
                            break;
                    }
                }
                finally
                {
                    IsWorking = false;
                }
            }, () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand TakeAndDisplayPhotoCommand { get; private set; }
        #endregion

        #region properties
        public string StatusMessage
        {
            get;
            set => SetProperty(ref field, value);
        } = "CameraInit".GetStringFromResource();

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