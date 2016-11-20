using NLog;
using System;

namespace ServiceFramework.Processors
{
    public sealed class TestProcessor : ProcessorBase
    {
        private readonly int processingDelay;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public TestProcessor(int processingDelay)
        {
            this.processingDelay = processingDelay;
        }

        public override string Name
        {
            get
            {
                return Constants.ShortServiceName + ".TestProcessor";
            }
        }

        public override int ProcessingDelay
        {
            get
            {
                return processingDelay;
            }
        }

        public override void Cleanup()
        {
            // it is not required to do anything here yet
        }

        public override void ExecuteHealthCheck()
        {
            // it is not required to do anything here yet
        }

        public override void Process()
        {
            try
            {
                Logger.Info("Look at me I am processing!");
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "Error while processing");
            }
        }
    }
}