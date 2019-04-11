using CreSync.ServiceCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoteCore.Components
{
    public class Vote
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Topic Topic { get; set; } = null;
        public Subject Subject { get; set; } = null;
        public Option Option { get; set; } = null;
        public string VoteUser { get; set; } = null;
        public DateTime VoteDate { get; set; } = DateTime.Now;
    }
}