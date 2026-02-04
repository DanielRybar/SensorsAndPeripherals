using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class GpsViewModel : BaseViewModel
    {
        private readonly IGeolocationService geolocationService = DependencyService.Get<IGeolocationService>()!;
        public GpsViewModel()
        {
            GetLastKnownCachedLocationCommand = new Command(async () =>
            {
                IsWorking = true;
                var (status, location) = await geolocationService.GetLastKnownCachedLocation();
                ResultText = $"Status: {status}, Location: {location}";
                IsWorking = false;
            });
            GetCurrentFineLocationCommand = new Command(async () =>
            {
                IsWorking = true;
                var (status, location) = await geolocationService.GetCurrentFineLocation();
                ResultText = $"Status: {status}, Location: {location}";
                IsWorking = false;
            });
            GetCurrentCoarseLocationCommand = new Command(async () =>
            {
                IsWorking = true;
                var (status, location) = await geolocationService.GetCurrentCoarseLocation();
                ResultText = $"Status: {status}, Location: {location}";
                IsWorking = false;
            });
        }

        public ICommand GetLastKnownCachedLocationCommand { get; private set; }
        public ICommand GetCurrentFineLocationCommand { get; private set; }
        public ICommand GetCurrentCoarseLocationCommand { get; private set; }

        public void CancelCurrentLocationRequest() => geolocationService.CancelCurrentLocationRequest();

        public string ResultText { get; set => SetProperty(ref field, value); } = string.Empty;
    }
}