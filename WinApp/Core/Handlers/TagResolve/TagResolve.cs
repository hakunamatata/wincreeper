using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Core.Handlers
{
    public abstract class TagResolve : IResourceTag
    {
        public string Name { get; set; }

        public string ResourceAttribute { get; set; }
        public bool AutoClose { get; set; }
        public TagResolve(string name, string attr, bool autoClose)
        {
            Name = name;
            ResourceAttribute = attr;
            AutoClose = AutoClose;
        }
        public TagResolve(string name, string attr) : this(name, attr, true)
        {

        }

        public string Resolve(string url)
        {
            var tag = $"<{Name} {ResourceAttribute}=\"{url}\"";
            if (AutoClose)
                tag += " />";
            else
                tag += $"></{Name}>";
            return tag;
        }
    }
}
