using SensorsAndPeripherals.Helpers;
using SensorsAndPeripherals.ViewModels.Abstract;
using System.Windows.Input;

namespace SensorsAndPeripherals.ViewModels
{
    public class AboutAppViewModel : BaseViewModel
    {
        #region constructor
        public AboutAppViewModel()
        {
            SendFeedbackCommand = new Command(async () =>
            {
                string subject = "FeedbackEmailSubject".GetStringFromResource();
                string body = "FeedbackEmailBody".GetStringFromResource();
                var msg = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    BodyFormat = EmailBodyFormat.PlainText,
                    To = ["EmailDR".GetStringFromResource()]
                };
                await Email.Default.ComposeAsync(msg);
            });
        }
        #endregion

        #region commands
        public ICommand SendFeedbackCommand { get; private set; }
        #endregion
    }
}