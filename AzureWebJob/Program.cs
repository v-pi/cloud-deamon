using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using CloudDaemon.Common.Interfaces;
using CloudDaemon.Common.Entities;
using HtmlAgilityPack;
using CloudDaemon.Common.Impl;
using CloudDaemon.DAL;

namespace CloudDaemon.AzureWebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    public class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        public static void Main()
        {
            try
            {
#if !DEBUG
                LogManager.Logger = new OneTimeLogger(new ProfileRepository());
#endif

                LogManager.Log("Start !");
                JobHostConfiguration config = new JobHostConfiguration();
                config.Queues.BatchSize = 1;
                var host = new JobHost(config);

                Init();
                Run();
#if DEBUG
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
#else
                LogManager.Log("This is release !");
                // The following code ensures that the WebJob will be running continuously
                //host.RunAndBlock();
#endif
            }
            catch (Exception ex)
            {
                LogManager.Log(ex);
            }
            finally
            {
                LogManager.FlushLogger();
            }
        }

        // TODO : Put most of this class in Common
        public static void Init()
        {
            // There are some weird non html things inside those script tags. Do NOT look into them if you wanna parse
            HtmlNode.ElementsFlags.Remove("script");
            // kinda bad : password is stored in static var... eh whatever
            EmailNotifier.SenderProfile = new ProfileRepository().GetProfileByAlias("Automated Notification");
        }

        public static void Run()
        {
            IMonitorManager monitorManager = new MonitorRepository();
            IResultHandlerManager resultHandlerManager = new ResultHandlerRepository();
            DateTime runTime = DateTime.Now;

            IEnumerable<MonitorEntity> monitorEntities = monitorManager.GetAllMonitors();
            foreach (MonitorEntity monitorEntity in monitorEntities)
            {
                // If the monitor has run recently, do nothing (we remove 10 seconds to the last run time to account for delayed starts)
                if (monitorEntity.LastRun + monitorEntity.Frequency - new TimeSpan(0, 0, 10) > DateTime.Now ||
                    !monitorEntity.IsActivated)
                    continue;

                monitorEntity.LastRun = runTime;

                IMonitor monitor = (IMonitor)Activator.CreateInstance(monitorEntity.MonitorAssembly, monitorEntity.MonitorName).Unwrap();
                IEnumerable<ResultHandlerEntity> resultHandlers = resultHandlerManager.GetResultHandlers(monitorEntity.IdMonitor);
                // An authentified monitor is a monitor with an identity (profile)
                if (monitor is AuthentifiedMonitor)
                    ((AuthentifiedMonitor)monitor).Profile = monitorEntity.Profile;
                foreach (ResultHandlerEntity resultHandlerEntity in resultHandlers)
                {
                    IResultHandler resultHandler = (IResultHandler)Activator.CreateInstance(resultHandlerEntity.ResultHandlerAssembly, resultHandlerEntity.ResultHandlerName).Unwrap();
                    // A notifier is a resulthandler that notifies someone (profile)
                    if (resultHandler is Notifier)
                        ((Notifier)resultHandler).Profile = resultHandlerEntity.Profile;
                    monitor.MonitorEnded += resultHandler.HandleResult;
                }
                monitor.Monitor();

                monitorManager.UpdateLastRunTime(monitorEntity);
            }
        }
    }
}