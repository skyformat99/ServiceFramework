using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceFramework.Processors
{
    public abstract class ProcessorBase : IProcessor
    {
        private readonly CancellationTokenSource cancellationToken;
        private readonly Task processingTask;

        protected ProcessorBase()
        {
            cancellationToken = new CancellationTokenSource();
            processingTask = new Task(ProcessingLoop);
        }

        private void ProcessingLoop()
        {
            var interval = TimeSpan.Zero;
            while (!cancellationToken.Token.WaitHandle.WaitOne(interval))
            {
                try
                {
                    Process();
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex, "ProcessingLoop()");
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                interval = new TimeSpan(0, 0, 0, 0, ProcessingDelay);
            }
        }

        private static readonly Logger Logger = LogManager.GetLogger(Constants.ShortServiceName);

        public abstract string Name { get; }

        public abstract int ProcessingDelay { get; }

        public abstract void Cleanup();

        public abstract void ExecuteHealthCheck();

        public abstract void Process();

        public virtual void Start()
        {
            Logger.Info("Starting processor " + Name);
            processingTask.Start();
            Logger.Info("Starting processor " + Name + " - done");
        }

        public virtual void Stop()
        {
            Logger.Info("Stopping processor " + Name);
            cancellationToken.Cancel();
            if (processingTask != null)
            {
                processingTask.Wait();
            }
            Cleanup();
            Logger.Info("Stopping processor " + Name + " - done");
        }
    }
}