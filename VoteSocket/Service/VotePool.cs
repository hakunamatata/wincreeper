using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoteCore.Components;

namespace VoteSocket.Service
{
    public class VotePool
    {
        public Dictionary<string, Vote> Pool { get; private set; }
        public VotePool(string topicId)
        {
            Pool = new Dictionary<string, Vote>();
        }

        //private List<Vote> getVotesOfTopic(string topicId)
        //{

        //}
    }
}
