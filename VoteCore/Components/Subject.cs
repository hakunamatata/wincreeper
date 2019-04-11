using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoteCore.Components
{
    public enum VoteOptionMode
    {
        Single = 1,
        Multiple = 2
    }

    public class Subject
    {
        public string Id {get; set;} = Guid.NewGuid().ToString();
        public Topic Topic {get; set;} = null;
        public string Title {get; set;} = string.Empty;
        public string EnglishTitle {get; set;} = string.Empty;
        public string Media { get; set; } = string.Empty;
        public List<Option> Options {get; set;} = null;
        public VoteOptionMode OptionMode {get; set;} = VoteOptionMode.Single;
        public int MaxOptions {get; set;} = 3;
        public int Order {get; set;} = 99;
        public Status Status {get; set;} = Status.Actived;

    }
}