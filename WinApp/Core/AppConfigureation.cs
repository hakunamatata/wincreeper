using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Core
{
    public class AppConfiguration
    {
        private static string appRoot => Environment.CurrentDirectory;
        private static string configFile => $"{appRoot}\\app.json";
        public static AppConfiguration Current
        {
            get
            {
                try {
                    return JsonConvert.DeserializeObject<AppConfiguration>(File.ReadAllText(configFile));
                }
                catch {
                    var config = new AppConfiguration();
                    File.WriteAllText(configFile, JsonConvert.SerializeObject(config));
                    return config;
                }
            }
        }
        public string ApplicationName { get; set; } = "Wechat Content Helper";
#if (DEBUG)
        public string Version { get; set; } = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
#else
        public string Version { get; set; } = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
#endif
        public Settings AppSettings { get; set; } = new Settings();

        public void UpdateConfig()
        {
            File.WriteAllText(configFile, JsonConvert.SerializeObject(this));
        }
    }

    public class Settings
    {
        public List<RecentSetting> Recent { get; set; } = new List<RecentSetting>();

        public DownloadSeting SaveConfig { get; set; } = new DownloadSeting();

        public List<string> WechatResourceDomain { get; set; } = new List<string>() { "https://mmbiz.qpic.cn"};
    }

    public class RecentSetting
    {
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
        public string Type { get; set; } = "";
        public RecentSetting()
        {

        }
    }

    public class DownloadSeting
    {
        public string RootLocalDirectory { get; set; } = "";
        public bool LocalIndividulelyDirecotry { get; set; } = false;
        public string RemoteDirectory { get; set; } = "";
    }

}
