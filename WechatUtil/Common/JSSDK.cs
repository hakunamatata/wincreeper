using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace WechatUtil.Common
{
    public class JSSDK
    {
        static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static WechatWebConfig GetWxConfig(string url)
        {
            var config = new WechatWebConfig {
                appid = ConfigurationManager.AppSettings["appid"],
                oauthid = ConfigurationManager.AppSettings["oauthId"],
                nonce = GetNonce(),
                ticket = JSAPITicket.GetTicket(),
                timestamp = GetTimestamp(),
                url = url
            };
            config.signature = GetSignature(config.ticket, config.timestamp, config.nonce, url);
            return config;
        }
        public static string GetJSApiTicket()
        {
            return JSAPITicket.GetTicket();
        }

        public static string GetNonce()
        {
            return Cryptography.RandomString(16, RandomFormatter.Default);
        }

        public static string GetTimestamp()
        {
            return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
        }
        public static string GetSignature(string ticket, string timespan, string nonceStr, string url)
        {
            var _ = new StringBuilder();
            _.Append("jsapi_ticket=").Append(ticket).Append("&")
                          .Append("noncestr=").Append(nonceStr).Append("&")
                          .Append("timestamp=").Append(timespan).Append("&")
                          .Append("url=").Append(url.IndexOf("#") >= 0 ? url.Substring(0, url.IndexOf("#")) : url);
            // 加密
            SHA1 sha;
            ASCIIEncoding enc;
            string hash = "";
            try {
                sha = new SHA1CryptoServiceProvider();
                enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(_.ToString());
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                return hash.ToLower();
            }
            catch (Exception) {
                return string.Empty;
            }
        }
    }

    public class WechatWebConfig
    {
        public string appid { get; set; }
        public string oauthid { get; set; }
        public string ticket { get; set; }
        public string nonce { get; set; }
        public string timestamp { get; set; }
        public string signature { get; set; }
        public string url { get; set; }
    }
}
