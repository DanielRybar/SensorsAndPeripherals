using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Interfaces.Peripherals;
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
                var photoFileResult = await peripheralService.TakePhotoAsync();
                if (photoFileResult is not null)
                {
                    var resultPath = await peripheralService.SavePhotoToCacheAsync(photoFileResult);
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
                else
                {
                    PhotoPath = null;
                    StatusMessage = "CameraShootingError".GetStringFromResource();
                }
                IsWorking = false;
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