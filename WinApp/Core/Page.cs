using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinApp.Components;

namespace WinApp.Core
{
    public class PageResolve
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Resource> Resources { get; set; } = new List<Resource>();
        public string RawHtml { get; set; }
        public string ResolvedHtml { get; set; }
        public Dictionary<string, string> ReplaceElements { get; private set; } = new Dictionary<string, string>();
        public PageResolve()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
