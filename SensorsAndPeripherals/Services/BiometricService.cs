using Plugin.Maui.Biometric;
using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.Models.Enums;

namespace SensorsAndPeripherals.Services
{
    public class BiometricService : IBiometricService
    {
        private readonly IBiometric auth = BiometricAuthenticationService.Default;

        public async Task<bool> IsAvailable()
        {
            return await auth.GetAuthenticationStatusAsync() == BiometricHwStatus.Success;
        }

        public async Task<BiometricType[]?> GetBiometricTypes()
        {
            if (await IsAvailable())
            {
                return await auth.GetEnrolledBiometricTypesAsync();
            }
            return null;
        }

        public async Task<BiometricAuthResult> Authenticate(string title, string description, string negativeText, AuthenticatorStrength strength)
        {
            if (!await IsAvailable())
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