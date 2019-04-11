using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteCore.Models.Sns
{
    public class JsCode2SessionPost
    {
        public string appid { get; set; }
        public string secret { get; set; }
        public string js_code { get; set; }
        public string grand_type { get; set; } = "authorization_code";
    }
}
