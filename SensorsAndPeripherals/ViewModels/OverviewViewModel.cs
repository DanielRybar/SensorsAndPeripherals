using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.Models;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels
{
    public class OverviewViewModel : BaseViewModel
    {
        #region services
        private readonly ISensorListService sensorListService = DependencyService.Get<ISensorListService>();
        #endregion

        #region constructor
        public OverviewViewModel()
        {
            GetSensorsCommand = new Command(() =>
            {
                IsBusy = true;
                Sensors.Clear();
                var sensors = sensorListService.GetAllSensors();
                foreach (var sensor in sensors)
                {
                    Sensors.Add(sensor);
                }
                IsBusy = false;
            });
        }
        #endregion

        #region commands
        public ICommand GetSensorsCommand { get; private set; }
        #endregion

        #region properties
        public ObservableCollection<SensorInfo> Sensors
        {
            get;
            set => SetProperty(ref field, value);
        } = [];
        #endregion

    }
}