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
        private readonly string defaultMessageValue = "NfcDefaultValue".GetStringFromResource();
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
        public event Action<string?>? ShowReadingResultRequested;
        #endregion

        #region methods
        public void CancelCurrentRequests()
        {
            peripheralService.CancelCurrentRequests();
            StatusMessage = string.Empty;
            if (!IsMessageToWriteValid)
            {
                MessageToWrite = defaultMessageValue;
            }
        }

        private async Task PerformScanAsync()
        {
            StatusMessage = "NfcPerformScan".GetStringFromResource();
            var (status, content) = await peripheralService.ScanAsync();
            switch (status)
            {
                case NfcStatus.Success:
                    StatusMessage = string.Empty;
                    ShowReadingResultRequested?.Invoke(content);
                    break;
                case NfcStatus.NotSupported:
                    StatusMessage = "NFCNotSupported".GetStringFromResource();
                    break;
                case NfcStatus.NotEnabled:
                    StatusMessage = "NfcNotEnabled".GetStringFromResource();
                    break;
                case NfcStatus.ReadEmpty:
                    StatusMessage = "NfcEmpty".GetStringFromResource();
                    break;
                case NfcStatus.OperationCancelled:
                    StatusMessage = "NfcOperationCancelled".GetStringFromResource();
                    break;
                case NfcStatus.UnknownError:
                default:
                    StatusMessage = "NfcReadingError".GetStringFromResource();
                    break;
            }
        }

        private async Task PerformWriteAsync()
        {
            if (!IsMessageToWriteValid)
            {
                await Toast.Make(
                    $"{"NfcIsMessageToWriteValid1".GetStringFromResource()} {MaxTextLength} {"NfcIsMessageToWriteValid2".GetStringFromResource()}").Show();
                return;
            }
            StatusMessage = "NfcPerformReading".GetStringFromResource();
            var status = await peripheralService.WriteAsync(MessageToWrite);
            StatusMessage = status switch
            {
                NfcStatus.Success => "NfcWriteSuccess".GetStringFromResource(),
                NfcStatus.NotSupported => "NFCNotSupported".GetStringFromResource(),
                NfcStatus.NotEnabled => "NfcNotEnabled".GetStringFromResource(),
                NfcStatus.WriteFailed => "NfcWriteFailed".GetStringFromResource(),
                NfcStatus.OperationCancelled => "NfcOperationCancelled".GetStringFromResource(),
                _ => "NfcWriteError".GetStringFromResource(),
            };
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

        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Property is bound in the View.")]
        public int MaxTextLength => 100;
        #endregion
    }
}