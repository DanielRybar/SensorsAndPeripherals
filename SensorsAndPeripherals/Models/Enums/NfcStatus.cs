namespace SensorsAndPeripherals.Models.Enums
{
    public enum NfcStatus
    {
        Success = 1,
        NotSupported,
        NotEnabled,
        ReadEmpty,
        WriteFailed,
        OperationCancelled,
        UnknownError
    }
}
