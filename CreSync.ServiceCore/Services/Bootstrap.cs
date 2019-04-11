using CreSync.ServiceCore.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreSync.ServiceCore.Services
{
    public class Bootstrap : ServiceBase
    {
        public List<IService> Services { get; private set; }
        public Bootstrap(AppSetting appSetting) : base(appSetting.ServiceName, appSetting.ServiceDescription)
        {
            Services = appSetting.Services.Select(p => CreateService(p)).Where(p => p != null).ToList();
        }

        public override bool Initialize()
        {
            Debug.Print($"Bootstrap:正在初始化服务 ");
            Services.ForEach(srv => srv.Initialize());
            return true;
        }

        public override void Start()
        {
            Debug.Print($"Bootstrap:正在启动所有服务");
            Services.ForEach(srv => srv.Start());
        }
        public override void Execute()
        {
        }

        public override void Stop()
        {
            Debug.Print($"Bootstrap:正在停止服务 ");
            Services.ForEach(srv => srv.Stop());
        }
        private IService CreateService(IServiceConfig config)
        {
            Type o = Type.GetType(config.Type);
            try {
                return (IService)Activator.CreateInstance(o, config);
            }
            catch (Exception ex) {
                Debug.Print("[Bootstrap] 创建服务失败: " + ex.Message);
                return null;
            }
        }
    }
}
