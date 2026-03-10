using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Interfaces.Sensors;
using SensorsAndPeripherals.Models.Enums;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;
using AuthenticatorStrength = Plugin.Maui.Biometric.AuthenticatorStrength;

namespace SensorsAndPeripherals.ViewModels.Sensors
{
    public class BiometricViewModel : BaseViewModel
    {
        #region services
        private readonly IBiometricService biometricService = DependencyService.Get<IBiometricService>();
        #endregion

        #region constructor
        public BiometricViewModel()
        {
            GetAvailableBiometricTypesCommand = new Command(async () =>
            {
                IsWorking = true;
                var result = await biometricService.GetBiometricTypesAsync();
                if (result is not null)
                {
                    ShowBiometricTypesDialogRequested?.Invoke(string.Join(", ", result.Select(t => t.ToString())));
                }
                else
                {
                    ShowBiometricTypesDialogRequested?.Invoke("BiometricSensorsNotFound".SafeGetResource<string>());
                }
                IsWorking = false;
            },
            () => IsSupported && !IsWorking);

            PerformStrongAuthenticationCommand = new Command(async () =>
            {
                await PerformAuthentication(AuthenticatorStrength.Strong);
            },
            () => IsSupported);

            PerformWeakAuthenticationCommand = new Command(async () =>
            {
                await PerformAuthentication(AuthenticatorStrength.Weak);
            },
            () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand GetAvailableBiometricTypesCommand { get; private set; }
        public ICommand PerformStrongAuthenticationCommand { get; private set; }
        public ICommand PerformWeakAuthenticationCommand { get; private set; }
        #endregion

        #region delegates
        public event Action<string>? ShowBiometricTypesDialogRequested;
        #endregion

        #region methods
        public async Task InitializeAsync()
        {
            IsWorking = true;
            IsSupported = await biometricService.IsAvailableAsync();
            IsWorking = false;
        }

        private async Task PerformAuthentication(AuthenticatorStrength strength)
        {
            IsWorking = true;
            var result = await biometricService.AuthenticateAsync(
                "PerformAuthentication01".SafeGetResource<string>(),
                "PerformAuthentication02".SafeGetResource<string>(),
                "PerformAuthentication03".SafeGetResource<string>(),
                strength);

            AuthResult = result switch
            {
                BiometricAuthResult.Success => "BiometricStatusSuccess".SafeGetResource<string>(),
                BiometricAuthResult.Failed => "BiometricStatusFailed".SafeGetResource<string>(),
                BiometricAuthResult.NotAvailable => "BiometricStatusNotAvailable".SafeGetResource<string>(),
                _ => "BiometricStatusUnknown".SafeGetResource<string>()
            };
            IsWorking = false;
        }

        private void ChangeCanExecute()
        {
            ((Command)GetAvailableBiometricTypesCommand).ChangeCanExecute();
            ((Command)PerformStrongAuthenticationCommand).ChangeCanExecute();
            ((Command)PerformWeakAuthenticationCommand).ChangeCanExecute();
        }
        #endregion

        #region properties
        public bool IsSupported
        {
            get;
            set
            {
                if (SetProperty(ref field, value))
                {
                    ChangeCanExecute();
                }
            }
        }

        public override bool IsWorking
        {
            get;
            set
            {
                if (SetProperty(ref field, value))
                {
                    ChangeCanExecute();
                }
            }
        }

        public string AuthResult
        {
            get;
            set => SetProperty(ref field, value);
        } = "BiometricInit".SafeGetResource<string>();
        #endregion
    }
}