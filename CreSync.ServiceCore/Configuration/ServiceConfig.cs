using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreSync.ServiceCore.Configuration
{
    public class ServiceConfig : IServiceConfig
    {
        public string Name { get; set; } = "未命名服务";
        public string Description { get; set; } = "";
        public string Type { get; set; } = "";
        public int Period { get; set; } = 60;
        public MailConfig Mail { get; set; } = new MailConfig();

    }

    public class MailConfig
    {
        public string Server { get; set; } = "smtp.163.com";
        public int Port { get; set; } = 25;
        public string Account { get; set; } = "xymbtc@163.com";
        public string Password { get; set; } = "angel520";
        public List<string> MailTo { get; set; } = new List<string>();
        public bool Inherit { get; set; } = false;
    }
}
