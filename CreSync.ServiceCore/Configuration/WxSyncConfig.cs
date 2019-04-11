using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreSync.ServiceCore.Configuration
{
    public class WxSyncConfig : ServiceConfig
    {
        public WxSyncConfig()
        {
            Name = "微信关注者同步服务";
            Description = $"每隔一段时间抓取微信关注着，更新至数据库";
            Type = "CreSync.ServiceCore.Services.WechatSynchronouse.WxSyncService,CreSync.ServiceCore";
        }
    }
}
