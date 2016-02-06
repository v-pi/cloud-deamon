using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Interfaces;
using System.Net;
using System.Net.Mail;

namespace CloudDaemon.Common.Impl
{
    public class EmailNotifier : Notifier
    {
        public static Profile SenderProfile;

        public override void HandleResult(object sender, object result)
        {
            MailAddress fromAddress = new MailAddress(SenderProfile.Login, SenderProfile.Alias);

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(SenderProfile.Login, SenderProfile.Password)
            };

            using (MailMessage email = new MailMessage(fromAddress, new MailAddress(Profile.Login, Profile.Alias))
            {
                Subject = ((IHasSubject)(result)).Subject,
                Body = ((IHasMessage)(result)).Message
            })
            {
                smtp.Send(email);
            }
        }
    }
}
