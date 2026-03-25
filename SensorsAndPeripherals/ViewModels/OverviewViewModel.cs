using SensorsAndPeripherals.Constants;
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
            GetSensorsCommand = new Command(async () =>
            {
                await ExecuteSafeAsync(async () =>
                {
                    await Task.Delay(DelayConstants.LONG_DELAY);
                    Sensors.Clear();
                    var sensors = sensorListService.GetAllSensors().OrderBy(x => x.Power);
                    foreach (var sensor in sensors)
                    {
                        if (sensor?.Name is not null)
                        {
                            Sensors.Add(sensor);
                        }
                    }
                });
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