

using CreSync.ServiceCore.Configuration;
using CreSync.ServiceCore.Data;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatUtil;

namespace CreSync.ServiceCore.Services.WechatSynchronouse
{
    public partial class WxSyncService : PeriodService
    {
        public WxSyncService(IServiceConfig config) : base(config)
        {
        }

        protected override void PeriodExecutionAsync(object arg)
        {
            Task.Run(async () => {
                var users = await WechatEngine.GetUserDetails();
                try {
                    Database.Connection.Execute(SQL_ADD_WXUSER, users.Select(p => new {
                        p.city,
                        p.country,
                        p.groupid,
                        p.headimgurl,
                        p.language,
                        p.nickname,
                        p.openid,
                        p.province,
                        p.qr_scene,
                        p.qr_scene_str,
                        p.remark,
                        p.sex,
                        p.subscribe,
                        p.subscribe_scene,
                        p.subscribe_time,
                        tagid_list = JsonConvert.SerializeObject(p.tagid_list),
                        p.unionid
                    }));
                }
                catch (Exception ex) {
                    Debug.Print(ex.Message);
                    log.Error(ex);
                }
            });
            base.PeriodExecutionAsync(arg);
        }
    }
}
