using Plugin.Maui.Biometric;
using SensorsAndPeripherals.Interfaces.Sensors;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Services.Sensors
{
    public class BiometricService : IBiometricService
    {
        private readonly IBiometric auth = BiometricAuthenticationService.Default;

        public async Task<bool> IsAvailableAsync()
        {
            return await auth.GetAuthenticationStatusAsync() == BiometricHwStatus.Success;
        }

        public async Task<BiometricType[]?> GetBiometricTypesAsync()
        {
            if (await IsAvailableAsync())
            {
                return await auth.GetEnrolledBiometricTypesAsync();
            }
            return null;
        }

        public async Task<BiometricAuthResult> AuthenticateAsync(string title, string description, string negativeText, AuthenticatorStrength strength)
        {
            if (!await IsAvailableAsync())
            {
                return BiometricAuthResult.NotAvailable;
            }
            var request = new AuthenticationRequest
            {
                AllowPasswordAuth = true,
                Title = title,
                Subtitle = string.Empty,
                Description = description,
                NegativeText = negativeText,
                AuthStrength = strength
            };
            try
            {
                var result = await auth.AuthenticateAsync(request, CancellationToken.None);
                return result?.Status switch
                {
                    BiometricResponseStatus.Success => BiometricAuthResult.Success,
                    _ => BiometricAuthResult.Failed,
                };
            }
            catch
            {
                return BiometricAuthResult.Failed;
            }
        }
    }
}