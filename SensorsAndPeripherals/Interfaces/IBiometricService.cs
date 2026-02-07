using Plugin.Maui.Biometric;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Interfaces
{
    public interface IBiometricService
    {
        Task<bool> IsAvailable();
        Task<BiometricType[]?> GetBiometricTypes();
        Task<BiometricAuthResult> Authenticate(string title, string description, string negativeText, AuthenticatorStrength strength);
    }
}