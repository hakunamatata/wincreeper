using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using VoteCore.Models.Sns;
using WechatUtil.Common.Helper;
using WechatUtil.Public.MiniApp;

namespace AustraliaVote.Controllers
{
    public class AuthController : ApiController
    {
        /// <summary>
        /// https://mini.artibition.cn/sns/jscode2session
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [Route("sns/jscode2session")]
        [HttpPost]
        public IHttpActionResult JsCode2SessionAsync([FromBody] JsCode2SessionPost post)
        {
            try {
                var http = new HttpHelper();
                string result = http.Get($"https://api.weixin.qq.com/sns/jscode2session?appid={post.appid}&secret={post.secret}&js_code={post.js_code}&grant_type=authorization_code", Encoding.UTF8);
                return Ok(JsonConvert.DeserializeObject<JsCode2SesstionResult>(result));
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }
        }
    }
}
