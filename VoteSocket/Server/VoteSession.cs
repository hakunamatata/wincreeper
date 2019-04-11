using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteSocket.Server
{
    public class VoteSession : AppSession<VoteSession>
    {
        protected override void OnSessionStarted()
        {
            base.Send("Welcome");
        }

        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            base.Send("Unknow reqeust");
        }

        protected override void HandleException(Exception e)
        {
            base.Send("Error: {0}", e.Message);
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            //add you logics which will be executed after the session is closed
            base.OnSessionClosed(reason);
        }
    }
}
