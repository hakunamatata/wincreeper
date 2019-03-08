using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Core.Handlers
{
    public abstract class ResourceSaveHandlerBase : IResponsedResource
    {
        protected AppConfiguration Configuration = AppConfiguration.Current;
        public Stream ResponsedStream { get; private set; }

        public Resource Resource { get; private set; }

        public ResourceSaveHandlerBase(Stream stream, Resource resource)
        {
            ResponsedStream = stream;
            Resource = resource;
        }

        public virtual void Save(string path, string fileName)
        {

        }
    }
}
