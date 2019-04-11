using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteCore.Components
{
    public class WxRelation
    {
        public string Id { get; set; }
        public Topic Topic { get; set; }
        public WxUser User { get; set; }
        public WxUser FromUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
