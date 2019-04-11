using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoteCore.Components;

namespace VoteSocket.Service
{
    public class TopicPool
    {
        public Dictionary<string, Topic> Pool { get; private set; }

        public TopicPool()
        {
            Pool = new Dictionary<string, Topic>();
        }
    }
}
