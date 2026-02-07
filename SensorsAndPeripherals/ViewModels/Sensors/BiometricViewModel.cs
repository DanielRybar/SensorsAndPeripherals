using SensorsAndPeripherals.Interfaces;
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
            Task.Run(async () =>
            {
                MainThread.BeginInvokeOnMainThread(() => IsWorking = true);
                var available = await biometricService.IsAvailable();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsSupported = available;
                    IsWorking = false;
                });
            });

            GetAvailableBiometricTypesCommand = new Command(async () =>
            {
                IsWorking = true;
                var result = await biometricService.GetBiometricTypes();
                if (result is not null)
                {
                    ShowBiometricTypesDialogRequested?.Invoke(string.Join(", ", result.Select(t => t.ToString())));
                }
                else
                {
                    ShowBiometricTypesDialogRequested?.Invoke("Žádné biometrické senzory nenalezeny.");
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
        private async Task PerformAuthentication(AuthenticatorStrength strength)
        {
            IsWorking = true;
            var result = await biometricService.Authenticate(
                "Ověření identity",
                "Pro pokračování je třeba ověřit vaši identitu pomocí biometrického ověření.",
                "Použít heslo",
                strength);

            AuthResult = result switch
            {
                BiometricAuthResult.Success => "Ověření úspěšné",
                BiometricAuthResult.Failed => "Ověření selhalo",
                BiometricAuthResult.NotAvailable => "Ověření není dostupné",
                _ => "Neznámý výsledek ověření"
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
        } = "Zatím neověřeno";
        #endregion
    }
}