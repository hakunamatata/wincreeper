using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteCore.Components
{
    /// <summary>
    /// 某篇主题的邀请用户排行
    /// </summary>
    public class TopicRank
    {
        public string openid { get; set; }
        public string nickname { get; set; }
        public string headimgurl { get; set; }
        public int rank { get; set; } = 0;
    }
}
