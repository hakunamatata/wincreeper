using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WinApp.Core.Handlers
{
    public class ImageSaveHandler : ResourceSaveHandlerBase
    {
        public ImageSaveHandler(Stream stream, Resource resource) : base(stream, resource)
        {
        }

        public override void Save(string path, string fileName)
        {
            #region === 微信图片连接特殊处理 === 
            //https://mmbiz.qpic.cn/mmbiz_png/WpXgPnbAFSIbA4nibWZ5d9NHWPUfuUtd3eBoXSRH6Ip94W2mZk61IribponWUoZSb6hu3iaO9PzLqbL7dbeAdxrWw/640?wx_fmt=png&tp=webp&wxfrom=5&wx_lazy=1&wx_co=1
            if (Configuration.AppSettings.WechatResourceDomain.Any(p => Resource.Url.Contains(p))) {
                // 微信连接
                var uri = new UriBuilder(Resource.Url);
                var query = HttpUtility.ParseQueryString(uri.Query);
                Resource.Extention = "." + query["wx_fmt"];
                Resource.Name = Guid.NewGuid().ToString();
                fileName = Resource.FileName;
            }
            #endregion
            Image.FromStream(ResponsedStream).Save(path + "\\" + fileName);
            //
            Resource.ResourceTag = new ImageTag("img", "src");
        }

        ///// <summary>
        ///// 根据Url获取参数值
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //private Dictionary<string, string> queryString(string url)
        //{

        //}
    }
}
