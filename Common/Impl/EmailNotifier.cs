using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Interfaces;
using System.Net;
using System.Net.Mail;

namespace CloudDaemon.Common.Impl
{
    public class EmailNotifier : Notifier
    {
        public static Profile SenderProfile;

        // TODO : Use OAuth 2 -_-
        public override void HandleResult(object sender, object result)
        {
            SenderProfile = Profile;
            MailAddress fromAddress = new MailAddress(SenderProfile.Login, SenderProfile.Alias);

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
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
