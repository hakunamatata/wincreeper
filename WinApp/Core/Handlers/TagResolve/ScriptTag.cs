using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Core.Handlers
{
    public class ScriptTag : TagResolve
    {
        public ScriptTag(string name, string attr) : base(name, attr, false)
        {
        }
    }
}
