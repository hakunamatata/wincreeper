using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WinApp.Core.Handlers
{
    public class ResourceSaveFactory
    {
        public static IResponsedResource Create(Stream stream, Resource resource)
        {
            var config = AppConfiguration.Current;
            var ctype = resource.ContentType.ToLower();
            // 微信资源特殊处理
            if (config.AppSettings.WechatResourceDomain.Any(p => resource.Url.Contains(p))) {
                processWechatResource(resource);

            }
            if (ctype.Contains("image/jpeg")
                || ctype.Contains("image/png")
                || ctype.Contains("image/gif")
              )
                return new ImageSaveHandler(stream, resource);
            return new TextSaveHandler(stream, resource);
        }

        private static void processWechatResource(Resource res)
        {
            var reg = new Regex("[&]*?wx_fmt=([^&]*)[^&]*", RegexOptions.IgnoreCase);
            if (reg.IsMatch(res.Url)) {
                var match = reg.Match(res.Url);
                if (match.Groups[1].Value == "jpeg")
                    res.Extention = ".jpg";
                else
                    res.Extention = "." + match.Groups[1].Value;
            }
        }
    }
}
