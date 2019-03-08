using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Core.Handlers
{
    public interface IResponsedResource
    {
        Stream ResponsedStream { get; }
        Resource Resource { get; }
        void Save(string path, string fileName);
    }
}
