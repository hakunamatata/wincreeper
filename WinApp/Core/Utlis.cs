using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Core
{
    public static class Utils
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Bitmap GenerateQRCode(string message)
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode(message, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(data);
            return qrCode.GetGraphic(8);
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static FileInfo ZipDirectory(string directory)
        {
            DirectoryInfo di = new DirectoryInfo(directory);
            var copyTo = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Creeper\\{di.Name}";
            if (!Directory.Exists(copyTo))
                Directory.CreateDirectory(copyTo);
            copyDirectory(directory, copyTo, true);
            di = new DirectoryInfo(copyTo);
            var helper = new ZipHelper();
            string outputFile = $"{copyTo}\\{di.Name}.zip";
            helper.ZipFileFromDirectory(directory, outputFile, 6);
            return new FileInfo(outputFile);
        }

        private static bool copyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
        {
            bool ret = false;
            try {
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                if (Directory.Exists(SourcePath)) {
                    if (Directory.Exists(DestinationPath) == false)
                        Directory.CreateDirectory(DestinationPath);

                    foreach (string fls in Directory.GetFiles(SourcePath)) {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath)) {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (copyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch (Exception ex) {
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 将一个文件以Post的方式上传至服务器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postFile"></param>
        /// <returns></returns>
        public static string Post(string url, FileInfo postFile)
        {
            string modelId = Guid.NewGuid().ToString();
            string updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string encrypt = "f933797501d6e2c3c7624b8a285e05v9";
            byte[] fileContents;

            using (var fs = new FileStream(postFile.FullName, FileMode.Open, FileAccess.Read)) {
                fileContents = new byte[fs.Length];
                fs.Read(fileContents, 0, Convert.ToInt32(fs.Length));
            }

            string boundary = "PostPage";
            string enter = "\r\n";

            string modelIdStr = $"--{boundary}{enter}Content-Disposition: form-data; name=\"modelId\"{enter}{enter}{modelId}{enter}";
            string fileContentStr = $"--{boundary}{enter}Content-Type:application/octet-stream{enter}Content-Disposition: form-data; name=\"fileContent\"; filename=\"{postFile.Name}\"{enter}{enter}";
            string updateTimeStr = $"{enter}--{boundary}{enter}Content-Disposition: form-data;name=\"updateTime\"{enter}{enter}{updateTime}";
            string encryptStr = $"{enter}--{boundary}{enter}Content-Disposition: form-data; name=\"encrypt\"{enter}{enter}{encrypt}{enter}--{boundary}--";

            var modelIdStrBytes = Encoding.UTF8.GetBytes(modelIdStr);
            var fileContentStrBytes = Encoding.UTF8.GetBytes(fileContentStr);
            var updateTimeStrBytes = Encoding.UTF8.GetBytes(updateTimeStr);
            var encryptStrBytes = Encoding.UTF8.GetBytes(encryptStr);

            var request = WebRequest.Create(url);
            request.ContentType = $"multipart/form-data; boundary={boundary}";
            request.Method = "POST";

            try {
                using (var stream = request.GetRequestStream()) {
                    stream.Write(modelIdStrBytes, 0, modelIdStrBytes.Length);
                    stream.Write(fileContentStrBytes, 0, fileContentStrBytes.Length);
                    stream.Write(fileContents, 0, fileContents.Length);
                    stream.Write(updateTimeStrBytes, 0, updateTimeStrBytes.Length);
                    stream.Write(encryptStrBytes, 0, encryptStrBytes.Length);
                    using (var response = request.GetResponse()) {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                            return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex) {
                return "FAILED";
            }


        }
        /// <summary>
        /// 获取图片缩略图,并保存在指定文件夹
        /// </summary>
        /// <param name="image"></param>
        /// <param name="saveAs"></param>
        public static Image Thumbnail(this Image image, string saveAs = "")
        {
            var thumb = image.GetThumbnailImage(200, 200, new Image.GetThumbnailImageAbort(() => false), IntPtr.Zero);
            if (!string.IsNullOrEmpty(saveAs))
                thumb.Save(saveAs);
            return thumb;
        }

        /// <summary>
        /// 删除文件夹中的一切
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static void DeleteDirectory(this DirectoryInfo directory)
        {
            try {
                FileSystemInfo[] fileinfo = directory.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo) {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else {
                        //如果 使用了 streamreader 在删除前 必须先关闭流 ，否则无法删除 sr.close();
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e) {
                throw;
            }
        }
    }
}
