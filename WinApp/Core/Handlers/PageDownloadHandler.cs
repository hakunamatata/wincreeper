using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WinApp.Components;

namespace WinApp.Core.Handlers
{
    /// <summary>
    /// 网页同步器, 用来同步网页生成本地静态文件
    /// </summary>
    public class PageDownloadHandler : ProcessableHandler
    {
        private AppConfiguration configuration => AppConfiguration.Current;
        public PageDownloadHandler(ProcessableProgressBar progress, AppConfiguration appConfigueration) : base(progress)
        {
        }
        public PageDownloadHandler(ProcessableProgressBar progress) : this(progress, AppConfiguration.Current)
        {

        }

        public async Task DownloadPageAsync(PageResolve page)
        {
            var resolveProgress = 10;
            var progressMax = (int)(page.Resources.Count * 1.25) + resolveProgress;
            var saveProgress = progressMax - page.Resources.Count;
            var rootPath = $"{configuration.AppSettings.SaveConfig.RootLocalDirectory}\\{page.Id}";
            Ready(progressMax);
            Update(resolveProgress);
            for (var i = 0; i < page.Resources.Count; i++) {
                var r = page.Resources[i];
                Debug.Print($"=== Content-Type: {r.ContentType}  Url: {r.Url} ===");
                try {
                    var resourceLocalDir = $"{rootPath}\\assets";
                    if (!Directory.Exists(resourceLocalDir))
                        Directory.CreateDirectory(resourceLocalDir);
                    await downloadResource(r, resourceLocalDir);
                    r.ResolvedUrl = "./assets/" + r.FileName;
                    var uri = new UriBuilder(r.Url);
                    uri.Query = null;
                    if (page.RawHtml.Contains($"{uri.Path}")) {
                        var strRegImage = $@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\S]*{uri.Path}[\s\t\r\n]*[^<>]*?/?[\s\t\r\n]*>";
                        var srcReg = new Regex(strRegImage, RegexOptions.IgnoreCase);
                        if (srcReg.IsMatch(page.RawHtml)) {
                            page.RawHtml = srcReg.Replace(page.RawHtml, r.ResourceTag.Resolve(r.ResolvedUrl));
                        }
                    }
                }
                catch (Exception ex) {
                    Debug.Print($"[WARN]WinApp.Core.Handlers.DownloadPageAsync: can't download resource \"{r.Name}\" \"{r.Url}\", Details: {ex.Message}");
                }

                Update(1);
            }
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
            File.WriteAllText(rootPath + "\\index.html", page.RawHtml);
            Update(saveProgress);
        }

        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="res"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task downloadResource(Resource res, string path)
        {
            identityResource(res);
            //过滤掉没有名称资源文件, 可能是重定向资源
            if (string.IsNullOrEmpty(res.Name)) return;

            var resUrl = res.Url;
            #region === 微信图片特殊处理 ===
            if (configuration.AppSettings.WechatResourceDomain.Any(p => res.Url.Contains(p))) {
                if (resUrl.Contains("&tp=webp"))
                    resUrl = resUrl.Replace("&tp=webp", "");
            }
            #endregion
            var request = WebRequest.Create(resUrl);
            var response = await request.GetResponseAsync();
            res.ContentType = response.ContentType;
            res.ContentLength = response.ContentLength;
            try {
                using (Stream stream = response.GetResponseStream()) {
                    ResourceSaveFactory.Create(stream, res).Save(path, res.FileName);
                }
            }
            catch (Exception ex) {
                Debug.Print("[ERROR]WinApp.Core.Handlers.PageDownloadHandler.downloadResource: " + ex.Message);
            }

        }
        /// <summary>
        /// 资源识别
        /// </summary>
        /// <param name="res"></param>
        private void identityResource(Resource res)
        {
            var ub = new UriBuilder(res.Url);
            var filename = ub.Uri.Segments.Last().Replace("/", "");
            if (!string.IsNullOrEmpty(filename)) {
                if (filename.Contains(".")) {
                    res.Name = filename.Split('.').First();
                    res.Extention = "." + filename.Split('.').Last();
                }
                else
                    res.Name = filename;

            }
        }

        private string getRawResourcePath(string resUrl)
        {
            try {
                return new UriBuilder(resUrl).Path;
            }
            catch {
                return resUrl;
            }
        }

    }
}
