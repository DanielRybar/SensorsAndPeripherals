using SensorsAndPeripherals.Interfaces.Generic;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Abstract
{
    public abstract class SensorViewModel<TService, TEventArgs> : BaseViewModel
        where TService : class, ISensorService<TEventArgs>
        where TEventArgs : EventArgs
    {
        #region services
        protected readonly TService sensorService = DependencyService.Get<TService>();
        #endregion

        #region constructor
        public SensorViewModel()
        {
            IsSupported = sensorService.IsSupported;
            IsMonitoring = sensorService.IsMonitoring;
            ToggleCommand = new Command(ToggleSensor, () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand ToggleCommand { get; private set; }
        #endregion

        #region event handlers
        protected abstract void OnReadingChanged(object? sender, TEventArgs e);
        #endregion

        #region methods
        private void ToggleSensor()
        {
            if (IsMonitoring)
                StopSensor();
            else
                StartSensor();
        }

        private void StartSensor()
        {
            sensorService.ReadingChanged += OnReadingChanged;
            sensorService.Start(DefaultSpeed);
            IsMonitoring = true;
        }

        public void StopSensor()
        {
            sensorService.Stop();
            sensorService.ReadingChanged -= OnReadingChanged;
            IsMonitoring = false;
        }
        #endregion

        #region properties
        protected virtual SensorSpeed DefaultSpeed => SensorSpeed.UI;

        public bool IsSupported
        {
            get;
            set => SetProperty(ref field, value);
        }

        public bool IsMonitoring
        {
            get;
            set => SetProperty(ref field, value);
        }

        public string DisplayX
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public string DisplayY
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public string DisplayZ
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;
        #endregion
    }
}