using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CreSync.ServiceCore.Configuration;
using log4net;

namespace CreSync.ServiceCore.Services
{
    public class PeriodService : ServiceBase
    {
        protected static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IServiceConfig Config;
        Timer timerPeriod;
        TimerCallback periodAction;

        public PeriodService(IServiceConfig config) : base(config)
        {
            Config = config;
        }

        public override void Execute()
        {
            periodAction = new TimerCallback(PeriodExecutionAsync);
            timerPeriod = new Timer(periodAction, null, 200, Config.Period * 1000);
        }

        protected virtual void PeriodExecutionAsync(object arg)
        {
            log.Info($"Period Service {ServiceName} Excuted.");
        }

    }
}
