using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Util;

namespace AustraliaVote.Controllers
{
    public class UploadController : ApiController
    {
        [Route("upload/static/page")]
        [HttpPost]
        public IHttpActionResult UploadStaticPage()
        {
            try {
                if (HttpContext.Current.Request.Files.Count <= 0)
                    throw new ArgumentNullException("no content");

                var postFile = HttpContext.Current.Request.Files[0];
                var saveDir = HttpContext.Current.Server.MapPath($"~/Static/Pages");
                if (!Directory.Exists(saveDir))
                    Directory.CreateDirectory(saveDir);

                var filePath = $"{saveDir}\\{postFile.FileName}";
                postFile.SaveAs(filePath);
                var zipHelper = new ZipHelper();

                var unzipDirecotry = string.Empty;
                if (postFile.FileName.Contains("."))
                    unzipDirecotry = postFile.FileName.Split('.')[0];
                else
                    unzipDirecotry = postFile.FileName;
                var unzipPath = $"{saveDir}\\{unzipDirecotry}";
                if (!Directory.Exists(unzipPath))
                    Directory.CreateDirectory(unzipPath);
                zipHelper.UnZip(filePath, unzipPath);
                System.IO.File.Delete(filePath);
                return Ok();
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }
        }
    }
}
