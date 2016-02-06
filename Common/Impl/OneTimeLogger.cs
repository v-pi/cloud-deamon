using CloudDaemon.Common.Entities;
using CloudDaemon.Common.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CloudDaemon.Common.Enum;

namespace CloudDaemon.Common.Impl
{
    public class OneTimeLogger : ILogger, IOneTimeLogger
    {
        private List<LogElement> Logs = new List<LogElement>();

        private LogLevel MinLogLevelForAlert = LogLevel.Error;

        private IProfileManager ProfileManager;

        public OneTimeLogger(IProfileManager profileManager)
        {
            ProfileManager = profileManager;
        }

        // TODO : Make sure this still works if the DB is down ?
        // Init sender profile for EmailNotifier
        // Replace ProfileRepository with "Repository" from ConfigurationManager.AppSettings
        public void FlushLogger()
        {
            // If there is at least one error, send the entire log
            if (Logs.Count(l => l.LogLevel <= MinLogLevelForAlert) > 0)
            {
                try
                {
                    EmailMessage email = new EmailMessage();
                    StringBuilder sb = new StringBuilder();
                    Logs.ForEach(l => sb.AppendLine(l.ToString()));
                    email.Subject = "CloudDaemon execution raised errors";
                    email.Message = sb.ToString();

                    EmailNotifier notifier = new EmailNotifier();
                    notifier.Profile = ProfileManager.GetProfileByAlias("Mail Perso");
                    notifier.HandleResult(null, email);
                }
                catch
                {
                    SmsMessage sms = new SmsMessage();
                    LogElement firstError = Logs.First(l => l.LogLevel <= MinLogLevelForAlert);
                    if (firstError.Log is Exception)
                        sms.Message = String.Format("CloudDaemon execution raised errors : {0}", ((Exception)firstError.Log).Message);
                    else
                        sms.Message = String.Format("CloudDaemon execution raised errors : {0}", firstError.Log);

                    SmsNotifier notifier = new SmsNotifier();
                    notifier.Profile = ProfileManager.GetProfileByAlias("SMS Perso");
                    notifier.HandleResult(null, sms);
                }
            }
        }

        public void Log(Exception ex)
        {
            Logs.Add(new LogElement(LogLevel.Error, ex));
        }

        public void Log(string message)
        {
            Logs.Add(new LogElement(LogLevel.Info, message));
        }
    }
}