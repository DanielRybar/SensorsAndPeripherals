using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Peripherals
{
    public class CameraViewModel : BaseViewModel
    {
        #region services
        private readonly ICameraService cameraService = DependencyService.Get<ICameraService>();
        #endregion

        #region constructor
        public CameraViewModel()
        {
            IsSupported = cameraService.IsSupported;
            TakeAndDisplayPhotoCommand = new Command(async () =>
            {
                IsWorking = true;
                var photoFileResult = await cameraService.TakePhotoAsync();
                if (photoFileResult is not null)
                {
                    var resultPath = await cameraService.SavePhotoToCacheAsync(photoFileResult);
                    if (!string.IsNullOrEmpty(resultPath))
                    {
                        PhotoPath = resultPath;
                    }
                    else
                    {
                        PhotoPath = null;
                        StatusMessage = "Došlo k chybě při ukládání fotografie.";
                    }
                }
                else
                {
                    PhotoPath = null;
                    StatusMessage = "Došlo k chybě při pořizování fotografie.";
                }
                IsWorking = false;
            }, () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand TakeAndDisplayPhotoCommand { get; private set; }
        #endregion

        #region properties
        public bool IsSupported
        {
            get;
            set => SetProperty(ref field, value);
        }

        public string StatusMessage
        {
            get;
            set => SetProperty(ref field, value);
        } = "Pro pořízení fotografie stiskněte tlačítko níže.";

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