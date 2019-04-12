using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Util;
using VoteCore.Components;
using VoteCore.Models.Sns;
using VoteCore.Services;
using WechatUtil.Common;
using WechatUtil.Common.Helper;
using WechatUtil.Public.MiniApp;
using IP2Region;
using System.Web;

namespace AustraliaVote.Controllers
{
    public class AuthController : ApiController
    {
        static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DbSearcher ipSearcher = new DbSearcher(HttpContext.Current.Server.MapPath("~/App_Data/ip2region.db"));
        private WxUserService userService;
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

        /// <summary>
        /// https://mini.artibition.cn/sns/wxconf
        /// </summary>
        /// <returns></returns>
        [Route("sns/wxconf")]
        [HttpPost]
        public IHttpActionResult GetWechatConfig([FromBody] UrlPost post)
        {
            try {
                if (string.IsNullOrEmpty(post.url))
                    throw new ArgumentNullException("url");
                return Ok(JSSDK.GetWxConfig(post.url));
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// https://mini.artibition.cn/sns/authbase
        /// </summary>
        /// <returns></returns>
        [Route("sns/authbase")]
        [HttpPost]
        public IHttpActionResult GetAuthBase(string code, string topic = "", string invite = "")
        {
            string ip = string.Empty;
            string region = string.Empty;
            string openid = string.Empty;
            string access_token = string.Empty;

            try {
                ip = Network.GetClientIP();
                region = ipSearcher.BinarySearch(ip)?.Region;
            }
            catch {
                region = string.Empty;
            }
            userService = new WxUserService();
            try {
                string[] keypair = Cache.GetCache<string[]>(code);
                if (keypair == null) {
                    var http = new HttpHelper();
                    string oauthId = ConfigurationManager.AppSettings["oauthId"];
                    string oauthSecret = ConfigurationManager.AppSettings["oauthSecret"];
                    string apiUrl = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={oauthId}&secret={oauthSecret}&code={code}&grant_type=authorization_code";
                    var result = http.Post(apiUrl, string.Empty, Encoding.UTF8, Encoding.UTF8).ToJson<dynamic>();
                    openid = result.openid;
                    access_token = result.access_token;
                    if (region.Length > 100) region = string.Empty;
                    userService.SaveOpenIdAndInvitation(openid, topic, invite, ip, region);
                    Cache.WriteCache<string[]>(code, new string[] { openid, access_token }, DateTime.Now.AddHours(8));
                }

                else {
                    openid = keypair[0];
                    access_token = keypair[1];
                }
                return Ok(new {
                    openid,
                    access_token
                });
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }
        }


        [Route("sns/userinfo")]
        [HttpGet]
        public IHttpActionResult GetUserInfo(string code)
        {
            string ip = string.Empty;
            string region = string.Empty;
            try {
                ip = Network.GetClientIP();
                region = ipSearcher.BinarySearch(ip)?.Region;
            }
            catch {
                region = string.Empty;
            }
            try {
                var keypair = Cache.GetCache<string[]>(code);
                if (keypair == null)
                    throw new ArgumentException("code 已过期");

                var http = new HttpHelper();
                string apiUrl = $"https://api.weixin.qq.com/sns/userinfo?access_token={keypair[1]}&openid={keypair[0]}&lang=zh_CN";
                var userInfo = http.Get(apiUrl, Encoding.UTF8).ToJson<WxUser>();
                if (region.Length > 100) region = string.Empty;
                userInfo.ip_address = ip;
                userInfo.ip_region = region;
                if (!string.IsNullOrEmpty(userInfo.openid)) {
                    userService = new WxUserService();
                    userService.SaveWechatUser(userInfo);
                }
                return Ok(userInfo);
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }
        }
    }
}
