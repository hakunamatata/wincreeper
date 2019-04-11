using WechatUtil.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatUtil.Common.Comm
{
    public class JSAPITicket
    {
        public static JSAPITicket _Ticket;


        public string ticket { get; set; }

        public int expires_in { get; set; }

        private DateTime createTicketTime = DateTime.Now;

        public int errcode { get; set; }
        public string errmsg { get; set; }
        /// <summary>
        /// 到期时间(防止时间差，提前1分钟到期)
        /// </summary>
        /// <returns></returns>
        public DateTime TookenOverdueTime
        {
            get { return createTicketTime.AddSeconds(expires_in - 60); }
        }

        /// <summary>
        /// 刷新Ticket
        /// </summary>
        public static void Renovate()
        {
            if (_Ticket == null)
            {
                GetNewTicket();
            }

            _Ticket.createTicketTime = DateTime.Now;
        }

        public static bool IsTimeOut()
        {
            if (_Ticket == null)
            {
                GetNewTicket();
            }

            return DateTime.Now >= _Ticket.TookenOverdueTime;
        }

        public static JSAPITicket GetNewTicket()
        {
            string strurl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

            HttpHelper http = new HttpHelper();

            string respone = http.Get(string.Format(strurl, Token.GetToken()), Encoding.UTF8);

            var ticket = respone.ToJson<JSAPITicket>();

            JSAPITicket._Ticket = ticket;

            return ticket;
        }

        public static string GetTicket()
        {
            if (_Ticket == null)
            {
                GetNewTicket();
            }
            return _Ticket.ticket;
        }
    }
}
