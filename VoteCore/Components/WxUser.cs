using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteCore.Components
{
    public class WxUser
    {
        public string openid { get; set; }
        public bool? subscribe { get; set; } = null;
        public string nickname { get; set; }
        public int? sex { get; set; } = null;
        public string language { get; set; } = "zh_CN";
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public int? subscribe_time { get; set; } = null;
        public string unionid { get; set; }
        public string remark { get; set; }
        public int? groupid { get; set; }
        public string tagid_list { get; set; }
        public string subscribe_scene { get; set; }
        public int? qr_scene { get; set; }
        public string qr_scene_str { get; set; }
        public string ip_address { get; set; }
        public string ip_region { get; set; }
        public List<WxRelation> fromInvites { get; set; }
    }
}
