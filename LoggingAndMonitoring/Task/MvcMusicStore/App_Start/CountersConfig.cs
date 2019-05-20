using MvcMusicStore.Infrastructure;
using PerformanceCounterHelper;
using System;
using System.Web.Mvc;

namespace MvcMusicStore.App_Start
{
    public class CountersConfig
    {
        public static void ConfigureCounters()
        {
            CounterHelper<PerfomanceCounters> counterHelper = DependencyResolver.Current.GetService(typeof(CounterHelper<PerfomanceCounters>)) as CounterHelper<PerfomanceCounters>;

            foreach (PerfomanceCounters counter in Enum.GetValues(typeof(PerfomanceCounters)))
            {
                counterHelper.Reset(counter);
            }
        }
    }
}