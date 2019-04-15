using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreSync.ServiceCore.Services.WechatSynchronouse
{
    public partial class WxSyncService
    {
        /// <summary>
        /// 添加微信用户，如果存在则更新， 如果不存在则添加
        /// </summary>
        private readonly string SQL_ADD_WXUSER = @"
            if (exists(select * from wx_users where openid=@openid))
                update wx_users set subscribe = @subscribe, nickname = @nickname, sex = @sex,[language] = @language, city = @city, province = @province, country = @country, headimgurl = @headimgurl, subscribe_time = @subscribe_time, unionid = @unionid, remark = @remark, groupid = @groupid, tagid_list = @tagid_list, subscribe_scene = @subscribe_scene, qr_scene = @qr_scene, qr_scene_str = @qr_scene_str, updateDate=getdate() where openid = @openid
            else
                insert wx_users values(@openid, @subscribe, @nickname, @sex, @language, @city, @province, @country, @headimgurl, @subscribe_time, @unionid, @remark, @groupid, @tagid_list, @subscribe_scene, @qr_scene, @qr_scene_str, getdate(),null)";
    }
}
