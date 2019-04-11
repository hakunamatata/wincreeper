using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatUtil.Common;

namespace WechatUtil
{
    public class GetUserRequest : OperationRequestBase<GetUserResult, HttpGetRequest>
    {
        protected override string Url()
        {
            return "https://api.weixin.qq.com/cgi-bin/user/get?access_token=ACCESS_TOKEN";
        }
        /// <summary>
        /// 第一个拉取的OPENID，不填默认从头开始拉取
        /// </summary>
        public string next_openid { get; set; }
    }

    public class GetUserResult : OperationResultsBase
    {
        public int total { get; set; }

        public int count { get; set; }

        public string next_openid { get; set; }

        public GetUserResultData data { get; set; }

        public class GetUserResultData
        {
            public List<string> openid { get; set; }
        }
    }
}
