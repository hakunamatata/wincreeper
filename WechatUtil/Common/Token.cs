using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatUtil.Common.Helper;
using System.Configuration;
using log4net;

namespace WechatUtil.Common
{
    public class Token
    {
        static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Token _Token;

        public string access_token { get; set; }

        public int expires_in { get; set; }

        public DateTime ExpiredAt { get; set; }

        public bool IsTimeout()
        {
            return DateTime.Now >= _Token.ExpiredAt;
        }
        /// <summary>
        /// 到期时间(防止时间差，提前1分钟到期)
        /// </summary>
        /// <returns></returns>
        public DateTime TookenOverdueTime
        {
            get { return ExpiredAt; }
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public static void Renovate()
        {
            if (_Token == null) {
                GetNewToken();
            }
        }

        public static bool IsTimeOut()
        {
            if (_Token == null) {
                GetNewToken();
            }

            return DateTime.Now >= Token._Token.ExpiredAt;
        }

        public static Token GetNewToken()
        {
            //string strulr = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
            string strurl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            string corpID = ConfigurationManager.AppSettings["appId"]; //公众号appId
            string Secret = ConfigurationManager.AppSettings["secret"];//公众号appSecret

            HttpHelper http = new HttpHelper();

            string respone = http.Get(string.Format(strurl, corpID, Secret), Encoding.UTF8);
            log.Debug("--------------- 向微信请求最新的Token ------------------");
            var token = respone.ToJson<Token>();
            token.ExpiredAt = DateTime.Now.AddSeconds(token.expires_in - 60);
            Token._Token = token;
            log.Debug(token.ToJson());
            log.Debug("------------------ Token请求完毕 ------------------");
            return token;
        }

        public static string GetToken()
        {
            if (_Token == null) {
                log.Debug("Token is null");
            }
            else {
                if (_Token.IsTimeout())
                    log.Debug("Token is expired.");
                log.Debug($"Token will expired at: {_Token.ExpiredAt}");
            }
            if (_Token == null || _Token.IsTimeout()) {
                GetNewToken();
            }
            return _Token.access_token;
        }
    }
}
