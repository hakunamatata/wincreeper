using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatUtil
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

        public string WechatAppId { get; set; } = "wx44460ea6602c89ba";
        public string WechatAppSecret { get; set; } = "b16280e45bdc87ad790716ded1d7a941";

    }
}
