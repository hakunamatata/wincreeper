using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatUtil.Common;

namespace WechatUtil
{
    public class BatchGetUserRequest : OperationRequestBase<BatchGetUserResult, HttpPostRequest>
    {
        protected override string Url()
        {
            return "https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token=ACCESS_TOKEN";
        }

        public List<BatchPostUser> user_list { get; set; }

        public BatchGetUserRequest()
        {
            user_list = new List<BatchPostUser>();
        }
    }

    public class BatchPostUser
    {
        public string openid { get; set; }
        public string lang { get; set; }
        public BatchPostUser()
        {
            lang = "zh_CN";
        }
        public BatchPostUser(string openId)
        {
            lang = "zh_CN";
            openid = openId;
        }
    }

    public class BatchGetUserResult : OperationResultsBase
    {
        public List<GetUserDetailResult> user_info_list { get; set; }
        public BatchGetUserResult()
        {
            user_info_list = new List<GetUserDetailResult>();
        }
    }
}
