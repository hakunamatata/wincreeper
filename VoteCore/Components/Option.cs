using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoteCore.Components
{
    public enum VoteReferenceType
    {
        Competitor = 1,
        Orgnization = 2
    }

    public class Option
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Subject Subject { get; set; } = null;
        public string Title { get; set; } = string.Empty;
        public string EnglishTitle { get; set; } = string.Empty;
        public string TitleImage { get; set; } = string.Empty;
        public Object Reference { get; set; } = null;
        public VoteReferenceType ReferenceType { get; set; } = VoteReferenceType.Competitor;
        public int Votes { get; set; } = 0;
        public int Order { get; set; } = 99;
        public Status Status { get; set; } = Status.Actived;

        public static readonly int DefaultOrder = 99;
    }
}