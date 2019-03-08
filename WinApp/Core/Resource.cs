using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinApp.Core.Handlers;

namespace WinApp.Core
{
    public class Resource
    {
        public string RawHtml { get; private set; }
        public string Url { get; set; }
        public string ResolvedUrl { get; set; }
        public string Name { get; set; }
        public string FileName => $"{Name}{Extention}";
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public string Extention { get; set; }
        public IResourceTag ResourceTag { get; set; }
        public Resource( string url, string extention = null)
        {
            Url = url;
            Extention = extention;

        }
    }
}
