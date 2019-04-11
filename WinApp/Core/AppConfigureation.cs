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
        public Settings AppSettings { get; set; } = new Settings();
        public string ConnectionString { get; set; } = "Server=trans.pub;Initial Catalog=wow002; UID=sa; PWD=sa@123";
        public void UpdateConfig()
        {
            File.WriteAllText(configFile, JsonConvert.SerializeObject(this));
        }
    }

    public class Settings
    {
        public List<RecentSetting> Recent { get; set; } = new List<RecentSetting>();

        public DownloadSeting SaveConfig { get; set; } = new DownloadSeting();

        public List<string> WechatResourceDomain { get; set; } = new List<string>() { "https://mmbiz.qpic.cn" };

        public string HostShareApi { get; set; } = "http://wxwu88.com/wrap?uid={ID}";
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
        public string RemotePath { get; set; } = "http://127.0.0.1:56373/static/pages";
        public string UploadAPI { get; set; } = "http://127.0.0.1:56373/upload/pages";
    }

}
