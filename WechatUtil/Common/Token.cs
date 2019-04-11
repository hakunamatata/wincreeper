using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatUtil.Common.Helper;
using System.Configuration;

namespace WechatUtil.Common
{
    public class Token
    {
        public static Token _Token;

        public string access_token { get; set; }

        public int expires_in { get; set; }

        private DateTime createTokenTime = DateTime.Now;

        /// <summary>
        /// 到期时间(防止时间差，提前1分钟到期)
        /// </summary>
        /// <returns></returns>
        public DateTime TookenOverdueTime
        {
            get { return createTokenTime.AddSeconds(expires_in - 60); }
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public static void Renovate()
        {
            if (_Token == null) {
                GetNewToken();
            }

            Token._Token.createTokenTime = DateTime.Now;
        }

        public static bool IsTimeOut()
        {
            if (_Token == null) {
                GetNewToken();
            }

            return DateTime.Now >= Token._Token.TookenOverdueTime;
        }

        public static Token GetNewToken()
        {
            //string strulr = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
            string strurl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            string corpID = AppSetting.Current.WechatAppId; //公众号appId
            string Secret = AppSetting.Current.WechatAppSecret;//公众号appSecret

            HttpHelper http = new HttpHelper();

            string respone = http.Get(string.Format(strurl, corpID, Secret), Encoding.UTF8);

            var token = respone.ToJson<Token>();

            Token._Token = token;

            return token;
        }

        public static string GetToken()
        {
            if (_Token == null) {
                GetNewToken();
            }
            return _Token.access_token;
        }
    }
}
