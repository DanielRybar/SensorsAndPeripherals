using Plugin.Maui.Biometric;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Interfaces
{
    public interface IBiometricService
    {
        Task<bool> IsAvailableAsync();
        Task<BiometricType[]?> GetBiometricTypesAsync();
        Task<BiometricAuthResult> AuthenticateAsync(string title, string description, string negativeText, AuthenticatorStrength strength);
    }
}