using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteCore.Components
{
    public class Result
    {
        public Topic Topic { get; set; }
        public Subject Subject { get; set; }
        public Option Option { get; set; }
        public int Votes { get; set; } = 0;
    }
}
