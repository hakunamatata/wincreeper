using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteSocket.Server.Commands
{
    public class vote : CommandBase<VoteSession, StringRequestInfo>
    {
        public override void ExecuteCommand(VoteSession session, StringRequestInfo requestInfo)
        {
        }
    }
}
