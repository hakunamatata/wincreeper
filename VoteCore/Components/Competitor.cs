using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoteCore.Components
{
    public class Competitor
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string AdvertUrl { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string EnglishTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string EnglishDescription { get; set; } = string.Empty;
        public Orgnization Orgnization { get; set; } = null;
        public string Source { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.Actived;

    }
}