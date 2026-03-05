using Plugin.NFC;
using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.Models.Enums;
using System.Diagnostics;

namespace SensorsAndPeripherals.Services.Peripherals
{
    public class NfcService : INfcService
    {
        private TaskCompletionSource<(NfcStatus, string?)>? scanTcs;
        private TaskCompletionSource<NfcStatus>? writeTcs;
        private string textToWrite = string.Empty;

        public bool IsSupported => CrossNFC.IsSupported;

        public NfcService()
        {
            if (IsSupported)
            {
                CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
                CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
                CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;
            }
        }

        public Task<(NfcStatus status, string? content)> ScanAsync()
        {
            if (!IsSupported)
            {
                return Task.FromResult((NfcStatus.NotSupported, (string?)null));
            }
            if (!CrossNFC.Current.IsAvailable || !CrossNFC.Current.IsEnabled)
            {
                return Task.FromResult((NfcStatus.NotEnabled, (string?)null));
            }
            CancelCurrentRequests();
            scanTcs = new TaskCompletionSource<(NfcStatus, string?)>();
            try
            {
                CrossNFC.Current.StartListening();
            }
            catch
            {
                return Task.FromResult((NfcStatus.UnknownError, (string?)null));
            }

            return scanTcs.Task;
        }

        public Task<NfcStatus> WriteAsync(string content)
        {
            if (!IsSupported)
            {
                return Task.FromResult(NfcStatus.NotSupported);
            }
            if (!CrossNFC.Current.IsAvailable || !CrossNFC.Current.IsEnabled)
            {
                return Task.FromResult(NfcStatus.NotEnabled);
            }
            CancelCurrentRequests();
            textToWrite = content;
            writeTcs = new TaskCompletionSource<NfcStatus>();
            try
            {
                CrossNFC.Current.StartPublishing();
                CrossNFC.Current.StartListening();
            }
            catch
            {
                return Task.FromResult(NfcStatus.UnknownError);
            }

            return writeTcs.Task;
        }

        public void CancelCurrentRequests()
        {
            StopNfcActivities();
            scanTcs?.TrySetResult((NfcStatus.OperationCancelled, null));
            writeTcs?.TrySetResult(NfcStatus.OperationCancelled);
            scanTcs = null;
            writeTcs = null;
        }

        private void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
        {
            if (writeTcs is not null && !writeTcs.Task.IsCompleted)
            {
                try
                {
                    var record = new NFCNdefRecord
                    {
                        TypeFormat = NFCNdefTypeFormat.WellKnown,
                        MimeType = "text/plain",
                        Payload = NFCUtils.EncodeToByteArray(textToWrite)
                    };
                    tagInfo.Records = [record];
                    CrossNFC.Current.PublishMessage(tagInfo, false);
                }
                catch
                {
                    StopNfcActivities();
                    writeTcs?.TrySetResult(NfcStatus.WriteFailed);
                    writeTcs = null;
                }
            }
        }

        private void Current_OnMessagePublished(ITagInfo tagInfo)
        {
            StopNfcActivities();
            writeTcs?.TrySetResult(NfcStatus.Success);
            writeTcs = null;
        }

        private void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            StopNfcActivities();
            string? recordText = tagInfo.Records?.FirstOrDefault()?.Message;
            var status = string.IsNullOrWhiteSpace(recordText) ? NfcStatus.ReadEmpty : NfcStatus.Success;
            scanTcs?.TrySetResult((status, recordText));
            scanTcs = null;
        }

        private static void StopNfcActivities()
        {
            try
            {
                CrossNFC.Current.StopListening();
                CrossNFC.Current.StopPublishing();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while stopping NFC activities " + ex.Message);
            }
        }
    }
}