using Dapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Core.ShareModule
{
    public class Share
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public Image Image { get; set; }
        public string Url { get; set; }

        public Image ShareInWechat()
        {
            // 添加到UrlAlias
            addToUrlAlias();
            return Utils.GenerateQRCode(AppConfiguration.Current.AppSettings.HostShareApi.Replace("{ID}", Id));
        }

        #region === Data Access === 
        void addToUrlAlias()
        {
            string query = "select Id, Title, Description, Image as IconPath, Url from Wechat_UrlAlias where Id=@Id";
            string updateQuery = "update Wechat_UrlAlias set @Title=@Title,Description=@Description,Image=@Image,Url=@Url where Id=@Id ";
            string insertQuery = "insert Wechat_UrlAlias Values(@Id,@Title,@Description,@Image,@Url)";
            var exists = Database.Connection.QueryFirstOrDefault<Share>(query, new { Id });
            if (exists != null) {
                Database.Connection.Execute(updateQuery, new { Id, Title, Description, Url, Image = IconPath });
            }
            else {
                Database.Connection.Execute(insertQuery, new { Title, Description, Url, Id, Image = IconPath });
            }
        }
        #endregion
    }
}
