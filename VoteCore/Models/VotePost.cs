using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoteCore.Models
{
    public class VotePost
    {
        public string uid { get; set; } = string.Empty;
        public VoteSubject[] subs { get; set; } = new VoteSubject[0];

    }

    public class VoteSubject
    {
        public string id { get; set; } = string.Empty;
        public SubjectOption[] ops { get; set; } = new SubjectOption[0];
    }

    public class SubjectOption
    {
        public string id { get; set; } = string.Empty;
        public int order { get; set; } = 99;
    }
}