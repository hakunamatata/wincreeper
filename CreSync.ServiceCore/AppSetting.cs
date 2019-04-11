using CreSync.ServiceCore.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreSync.ServiceCore
{
    public class AppSetting
    {
        private static string appRoot => Environment.CurrentDirectory;
        private static string configFile => $"{appRoot}\\app.json";
        public static AppSetting Current
        {
            get
            {
                try {
                    return JsonConvert.DeserializeObject<AppSetting>(File.ReadAllText(configFile));
                }
                catch {
                    var config = new AppSetting();
                    File.WriteAllText(configFile, JsonConvert.SerializeObject(config));
                    return config;
                }
            }
        }
        public string ServiceName { get; set; } = "微信综合业务";
        public string ServiceDescription { get; set; } = "同步微信粉丝等等功能";
        public string ConnectionString { get; set; } = "server=trans.pub;database=Creep;uid=sa;pwd=sa@123";
        public List<ServiceConfig> Services = new List<ServiceConfig>();
        public MailConfig Mail { get; set; } = new MailConfig();
        public AppSetting()
        {
            foreach (var srv in Services) {
                if (srv.Mail.Inherit)
                    srv.Mail = Mail;
            }
        }
    }
}
