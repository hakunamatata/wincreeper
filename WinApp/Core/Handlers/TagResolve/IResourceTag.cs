using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Core.Handlers
{
    public interface IResourceTag
    {
        /// <summary>
        /// 标记名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 资源地址属性名称
        /// </summary>
        string ResourceAttribute { get; }
        string Resolve(string url);
    }
}
