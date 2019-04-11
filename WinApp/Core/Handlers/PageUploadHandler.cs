using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApp.Components;

namespace WinApp.Core.Handlers
{
    public class PageUploadHandler : ProcessableHandler
    {
        AppConfiguration Configuration => AppConfiguration.Current;
        public PageUploadHandler(ProcessableProgressBar progressBar) : base(progressBar)
        {
        }

        public void Upload(string path)
        {
            Ready();
            // 压缩文档
            var postFile = Utils.ZipDirectory(path);
            Update(50);
            // 上传文档
            Utils.Post(Configuration.AppSettings.SaveConfig.UploadAPI, postFile);
            Update(40);
            postFile.Directory.DeleteDirectory();
            Update(10);
        }

    }
}
