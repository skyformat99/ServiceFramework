using NLog;
using System;
using System.Diagnostics;

namespace ServiceFramework
{
    public static class PerformanceCounters
    {
        private const string MessageRateCounterName = "Message rate";

        private const string MessageCountCounterName = "Message count";

        private const string LocalMachineName = ".";

        static PerformanceCounters()
        {
            if (PerformanceCounterCategory.Exists(Constants.PerformanceCategoryName))
            {
                PerformanceCounterCategory.Delete(Constants.PerformanceCategoryName);
            }

            if (!PerformanceCounterCategory.Exists(Constants.PerformanceCategoryName))
            {
                var counters = new CounterCreationDataCollection();

                var messageRate = new CounterCreationData
                {
                    CounterName = MessageRateCounterName,
                    CounterHelp = string.Empty,
                    CounterType = PerformanceCounterType.RateOfCountsPerSecond64
                };
                counters.Add(messageRate);

                var messageCount = new CounterCreationData
                {
                    CounterName = MessageCountCounterName,
                    CounterHelp = string.Empty,
                    CounterType = PerformanceCounterType.NumberOfItems64
                };
                counters.Add(messageCount);

                try
                {
                    PerformanceCounterCategory.Create(
                        Constants.PerformanceCategoryName,
                        Constants.PerformanceCategoryHelp,
                        PerformanceCounterCategoryType.SingleInstance,
                        counters);
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex, "Error while creating performance counters");
                    Environment.Exit(0);
                }
            }

            MessageRate = new PerformanceCounter
            {
                CategoryName = Constants.PerformanceCategoryName,
                CounterName = MessageRateCounterName,
                MachineName = LocalMachineName,
                ReadOnly = false
            };

            MessageCount = new PerformanceCounter
            {
                CategoryName = Constants.PerformanceCategoryName,
                CounterName = MessageCountCounterName,
                MachineName = LocalMachineName,
                ReadOnly = false
            };
        }

        private static readonly PerformanceCounter MessageRate;

        private static readonly PerformanceCounter MessageCount;

        private static readonly Logger Logger = LogManager.GetLogger(Constants.ShortServiceName);

        public static void IncrementMessageRate()
        {
            MessageRate.Increment();
        }

        public static void IncrementMessageCount()
        {
            MessageCount.Increment();
        }

        public static void Init()
        {
            // do nothing - used to create a static object instance
        }
    }
}