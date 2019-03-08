using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Core.Handlers
{
    public class TextSaveHandler : ResourceSaveHandlerBase
    {
        public TextSaveHandler(Stream stream, Resource resource) : base(stream, resource)
        {
        }

        public override void Save(string path, string fileName)
        {
            FileStream fs = new FileStream(path + "\\" + fileName, FileMode.Create, FileAccess.Write);
            byte[] buffer = new byte[Resource.ContentLength];
            ResponsedStream.Read(buffer, 0, buffer.Length);
            fs.Write(buffer, 0, buffer.Length);
        }
    }
}
