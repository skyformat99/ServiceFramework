using NLog;
using ServiceFramework.Processors;
using ServiceFramework.Properties;
using System;
using System.Configuration;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace ServiceFramework
{
    public partial class CoreService : ServiceBase
    {
        private static readonly Logger Logger = LogManager.GetLogger(Constants.ShortServiceName);

        private TestProcessor testProcessor;

        public CoreService()
        {
            InitializeComponent();
            DisplaySettings();
            PerformanceCounters.Init();
        }

        public void StartService()
        {
            // start processors
            testProcessor = new TestProcessor(Settings.Default.TestProcessorDelayInMilliSeconds);
            testProcessor.Start();
        }

        public void StopService()
        {
            if (testProcessor != null)
            {
                testProcessor.Stop();
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                StartService();
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Error while starting service");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        protected override void OnStop()
        {
            StopService();
        }

        private static void DisplaySettings()
        {
            Logger.Info(GetVersion());

            foreach (SettingsProperty sp in Settings.Default.Properties)
            {
                Logger.Info("Setting > " + sp.Name + " : " + Settings.Default[sp.Name]);
            }
        }

        private static string GetVersion()
        {
            return "Service version " + Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}