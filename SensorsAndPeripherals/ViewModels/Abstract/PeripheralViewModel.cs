using SensorsAndPeripherals.Interfaces.Base;

namespace SensorsAndPeripherals.ViewModels.Abstract
{
    public abstract class PeripheralViewModel<TService> : BaseViewModel 
        where TService : class, IPeripheralService
    {
        #region services
        protected readonly TService peripheralService = DependencyService.Get<TService>();
        #endregion

        #region constructor
        public PeripheralViewModel()
        {
            IsSupported = peripheralService.IsSupported;
        }
        #endregion

        #region properties
        public bool IsSupported
        {
            get;
            set => SetProperty(ref field, value);
        }
        #endregion
    }
}