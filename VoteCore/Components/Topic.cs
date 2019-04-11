using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteCore.Components
{
    public enum MediaType
    {
        Image = 1,
        Video = 2
    }
    public class Topic
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string EnglishTitle { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string EnglishSubTitle { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string EnglishContent { get; set; } = string.Empty;
        public string Media { get; set; } = string.Empty;
        public int MediaType { get; set; } = 1;
        public int Likes { get; set; } = 0;
        public List<Subject> Subjects { get; set; } = null;
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Actived;
    }
}
