using NLog;
using System;
using System.ServiceProcess;
using System.Threading;

namespace ServiceFramework
{
    public static class Program
    {
        private static readonly Logger Logger = LogManager.GetLogger(Constants.ShortServiceName);

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            if (args == null || args.Length == 0)
            {
                // service mode
                var servicesToRun = new ServiceBase[] { new CoreService() };
                ServiceBase.Run(servicesToRun);
            }
            else if (args[0] == "-debug")
            {
                // debug mode
                var service = new CoreService();
                service.StartService();
                Console.ReadKey();
                service.StopService();
                Environment.Exit(0);
            }
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Fatal((Exception)e.ExceptionObject, "Unhandled Exception");
            Thread.Sleep(5000);
            Environment.Exit(0);
        }
    }
}