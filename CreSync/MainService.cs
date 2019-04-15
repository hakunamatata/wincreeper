using CreSync.ServiceCore;
using CreSync.ServiceCore.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ServiceBase = System.ServiceProcess.ServiceBase;

namespace CreSync
{
    partial class MainService : ServiceBase
    {
        private Bootstrap m_Bootstrap;
        public MainService()
        {
            InitializeComponent();
            m_Bootstrap = new Bootstrap(AppSetting.Current);
        }
        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            if (!m_Bootstrap.Initialize())
                return;

            m_Bootstrap.Start();
        }
        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            m_Bootstrap.Stop();
            base.OnStop();
        }
        protected override void OnShutdown()
        {
            m_Bootstrap.Stop();
            base.OnShutdown();
        }

    }
}
