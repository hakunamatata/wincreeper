using WechatUtil.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace WechatUtil.Common
{
    public class JSAPITicket
    {
        static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static JSAPITicket _Ticket;

        public string ticket { get; set; }

        public int expires_in { get; set; }
        public DateTime ExpiredAt { get; set; }

        public bool IsTimeout()
        {
            return DateTime.Now >= _Ticket.ExpiredAt;
        }

        public int errcode { get; set; }
        public string errmsg { get; set; }

        public static JSAPITicket GetNewTicket()
        {
            string strurl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

            HttpHelper http = new HttpHelper();
            string respone = http.Get(string.Format(strurl, Token.GetToken()), Encoding.UTF8);
            log.Debug("--------------- 向微信请求最新的Ticket ------------------");
            var ticket = respone.ToJson<JSAPITicket>();
            ticket.ExpiredAt = DateTime.Now.AddSeconds(ticket.expires_in - 60);
            JSAPITicket._Ticket = ticket;
            log.Debug(ticket.ToJson());
            log.Debug("------------------ Ticket请求完毕 ------------------");
            return ticket;
        }

        public static string GetTicket()
        {
            if (_Ticket == null) {
                log.Debug("Ticket is null");
            }
            else {
                if (_Ticket.IsTimeout())
                    log.Debug("Ticket is expired.");
                log.Debug($"Ticket will expired at: {_Ticket.ExpiredAt}");
            }
            if (_Ticket == null || _Ticket.IsTimeout()) {
                GetNewTicket();
            }
            return _Ticket.ticket;
        }
    }
}
