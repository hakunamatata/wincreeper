using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatUtil.Common;

namespace WechatUtil.Public.MiniApp
{
    public class JsCode2SessionRequest : OperationRequestBase<JsCode2SesstionResult, HttpPostRequest>
    {
        private string appId { get; set; }
        private string secret { get; set; }
        public string code { get; set; }
        protected override string Url()
        {
            return "https://api.weixin.qq.com/sns/jscode2session?appid={appId}&secret={secret}&js_code={code}&grant_type=authorization_code";
        }

        public JsCode2SessionRequest(string appId, string secret, string code)
        {
            this.appId = appId;
            this.secret = secret;
            this.code = code;
        }
    }

    public class JsCode2SesstionResult : OperationResultsBase
    {
        public string openid { get; set; }
        public string session_key { get; set; }
        public string unionid { get; set; }
    }
}
