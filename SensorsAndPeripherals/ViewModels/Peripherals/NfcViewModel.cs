using CommunityToolkit.Maui.Alerts;
using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.Interfaces.Peripherals;
using SensorsAndPeripherals.Models.Enums;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels.Peripherals
{
    public class NfcViewModel : PeripheralViewModel<INfcService>
    {
        #region variables
        private readonly string defaultMessageValue = "NfcDefaultValue".SafeGetResource<string>();
        #endregion

        #region constructor
        public NfcViewModel()
        {
            MessageToWrite = defaultMessageValue;
            ScanCommand = new Command(async () => await PerformScanAsync(), () => IsSupported);
            WriteCommand = new Command(async () => await PerformWriteAsync(), () => IsSupported);
        }
        #endregion

        #region commands
        public ICommand ScanCommand { get; private set; }
        public ICommand WriteCommand { get; private set; }
        #endregion

        #region delegates
        public event Func<string?, Task>? ShowReadingResultRequested;
        #endregion

        #region methods
        public void CancelCurrentRequests()
        {
            peripheralService.CancelCurrentRequests();
            StatusMessage = string.Empty;
            IsInputEnabled = true;
            if (!IsMessageToWriteValid)
            {
                MessageToWrite = defaultMessageValue;
            }
        }

        private async Task PerformScanAsync()
        {
            StatusMessage = "NfcPerformScan".SafeGetResource<string>();
            var (status, content) = await peripheralService.ScanAsync();
            switch (status)
            {
                case NfcStatus.Success:
                    await ShowReadingResultRequested?.Invoke(content)!;
                    StatusMessage = string.Empty;
                    break;
                case NfcStatus.NotSupported:
                    StatusMessage = "NFCNotSupported".SafeGetResource<string>();
                    break;
                case NfcStatus.NotEnabled:
                    StatusMessage = "NfcNotEnabled".SafeGetResource<string>();
                    break;
                case NfcStatus.ReadEmpty:
                    StatusMessage = "NfcEmpty".SafeGetResource<string>();
                    break;
                case NfcStatus.OperationCancelled:
                    break;
                case NfcStatus.UnknownError:
                default:
                    StatusMessage = "NfcReadingError".SafeGetResource<string>();
                    break;
            }
        }

        private async Task PerformWriteAsync()
        {
            if (!IsMessageToWriteValid)
            {
                await Toast.Make(
                    $"{"NfcIsMessageToWriteValid1".SafeGetResource<string>()} {MaxTextLength} {"NfcIsMessageToWriteValid2".SafeGetResource<string>()}").Show();
                return;
            }

            IsInputEnabled = false;
            StatusMessage = "NfcPerformWriting".SafeGetResource<string>();
            try
            {
                var status = await peripheralService.WriteAsync(MessageToWrite);
                switch (status)
                {
                    case NfcStatus.Success:
                        StatusMessage = "NfcWriteSuccess".SafeGetResource<string>();
                        break;
                    case NfcStatus.NotSupported:
                        StatusMessage = "NFCNotSupported".SafeGetResource<string>();
                        break;
                    case NfcStatus.NotEnabled:
                        StatusMessage = "NfcNotEnabled".SafeGetResource<string>();
                        break;
                    case NfcStatus.WriteFailed:
                        StatusMessage = "NfcWriteFailed".SafeGetResource<string>();
                        break;
                    case NfcStatus.OperationCancelled:
                        break;
                    case NfcStatus.UnknownError:
                    default:
                        StatusMessage = "NfcWriteError".SafeGetResource<string>();
                        break;
                }
            }
            finally
            {
                IsInputEnabled = true;
            }
        }
        #endregion

        #region properties
        public string MessageToWrite
        {
            get;
            set => SetProperty(ref field, value);
        }

        public bool IsMessageToWriteValid
        {
            get;
            set => SetProperty(ref field, value);
        } = true;

        public bool IsInputEnabled
        {
            get;
            set => SetProperty(ref field, value);
        } = true;

        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Property is bound in the View.")]
        public int MaxTextLength => 100;
        #endregion
    }
}