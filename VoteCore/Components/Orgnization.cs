using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoteCore.Components
{
    public enum Status
    {
        Deleted = -1,
        Disabled = 0,
        Actived = 1
    }

    public class Orgnization
    {
        public string Id {get; set;} = Guid.NewGuid().ToString();
        public string Name {get; set;} = string.Empty;
        public string EnglishName {get; set;} = string.Empty;
        public string Description {get; set;} = string.Empty;
        public string EnglishDescription {get; set;} = string.Empty;
        public string Source { get; set; } = string.Empty;
        public Status Status {get; set;} = Status.Actived;
    }
}