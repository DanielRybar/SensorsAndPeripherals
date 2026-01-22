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

        public string Title
        {
            get;
            set => SetProperty(ref field, value);
        } = string.Empty;

        public bool IsWorking
        {
            get;
            set => SetProperty(ref field, value);
        } = false;
    }
}