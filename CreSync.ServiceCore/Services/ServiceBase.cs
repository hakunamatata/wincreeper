using CreSync.ServiceCore.Configuration;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CreSync.ServiceCore
{
    public abstract class ServiceBase : IService
    {
        protected static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Thread serviceThread;
        ThreadStart serviceStart;
        public string ServiceName { get; protected set; }

        public string ServiceDescription { get; protected set; }

        public virtual bool Initialize()
        {
            return true;
        }

        public virtual void Start()
        {
            serviceStart = new ThreadStart(runService);
            serviceThread = new Thread(serviceStart);
            log.Info($"{ServiceName} 正在启动...");
            serviceThread.Start();
        }

        public virtual void Stop()
        {
            serviceThread.Abort();
            log.Info($"{ServiceName} 已经停止...");
        }

        public ServiceBase(IServiceConfig config) : this(config.Name, config.Description)
        {

        }

        public ServiceBase(string name, string description)
        {
            ServiceName = name;
            ServiceDescription = description;
        }

        private void runService()
        {
            try {
                Execute();
            }
            catch (Exception ex) {
                log.Error($"{ServiceName} 运行错误", ex);
                Stop();
                restarServiceInDelay();
            }
        }

        private void restarServiceInDelay()
        {
            //10分钟后重新开启
            log.Info($"{ServiceName} 10分钟后重启...");
            Timer timer = new Timer(new TimerCallback((ojb) => {
                Start();
            }), null, 10 * 60 * 1000, Timeout.Infinite);
        }

        public abstract void Execute();

    }
}
