using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SensorsAndPeripherals.ViewModels.Abstract
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null!)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value)) return false;
            backingField = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        protected async Task ExecuteSafeAsync(Func<Task> operation)
        {
            if (IsWorking)
            {
                return;
            }
            IsWorking = true;
            try
            {
                await operation();
            }
            finally
            {
                IsWorking = false;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual bool IsWorking
        {
            get;
            set => SetProperty(ref field, value);
        } = false;
    }
}