using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoteCore.Components;

namespace VoteCore.Services
{
    public partial class WxUserService : BaseRepository
    {
        private readonly string SQL_TOPIC_RANK = @"
                select top 10 * from (
                select u.openid,u.nickname,u.headimgurl,count(p.Id) [rank]
                from [Vote].[dbo].[Vote_Pool] p
                inner join Wx_UserRelation r on p.VoteUser = r.UserId
                inner join Wx_User u on r.FromUserId = u.openid
                where p.TopicId = @topicId
                group by u.openid,u.nickname,u.headimgurl) as t
                order by t.[rank] desc";

        private readonly string SQL_USER_TOPIC_RANK = @"
                select top 10 * from (
                select u.openid,u.nickname,u.headimgurl,count(p.Id) [rank]
                from [Vote].[dbo].[Vote_Pool] p
                inner join Wx_UserRelation r on p.VoteUser = r.UserId
                inner join Wx_User u on r.FromUserId = u.openid
                where p.TopicId = @topicId and r.FromUserId=@userId
                group by u.openid,u.nickname,u.headimgurl) as t
                order by t.[rank] desc";
        private readonly string SQL_GET_USER = @"select * from Wx_User where openid=@openid";

        private readonly string SQL_SAVE_USER = @"
                if exists(select * from Wx_User where openid=@openid)
                update Wx_User set subscribe=@subscribe,nickname=@nickname,sex=@sex,[language]=@language,city=@city,province=@province,country=@country,headimgurl=@headimgurl,subscribe_time=@subscribe_time,unionid=@unionid,remark=@remark,groupid=@groupid,tagid_list=@tagid_list,subscribe_scene=@subscribe_scene,qr_scene=@qr_scene,qr_scene_str=@qr_scene_str,ip_address=@ip_address,ip_region=@ip_region where openid=@openid
                else
                insert Wx_User Values(@openid,@subscribe,@nickname,@sex,@language,@city,@province,@country,@headimgurl,@subscribe_time,@unionid,@remark,@groupid,@tagid_list,@subscribe_scene,@qr_scene,@qr_scene_str,@ip_address,@ip_region)";
        private readonly string SQL_SAVE_USER_OPENID = @"
               if not exists(select * from Wx_User where openid=@openid)
	              insert Wx_User(openid,ip_address,ip_region) Values(@openid,@ip_address,@ip_region)
               if (@topicId <> '' and @invite <> '' and @openid <> @invite) and (not exists(select * from Wx_UserRelation where TopicId=@topicId and UserId=@openid and FromUserId=@invite))
	              insert Wx_UserRelation(TopicId, UserId, FromUserId) Values(@topicId, @openid, @invite)";

        /// <summary>
        /// 仅当openid不存在与系统中是添加
        /// 用于用户第一次静默授权进入页面
        /// 仅保存非自身循环邀请且相同主题只允许邀请一次
        /// </summary>
        /// <param name="openid"></param>
        public void SaveOpenIdAndInvitation(string openid, string topicId = "", string invite = "", string ip_address = "", string ip_region = "")
        {
            if (string.IsNullOrEmpty(openid))
                throw new ArgumentException("必须提供用的openid");

            Connection.Execute(SQL_SAVE_USER_OPENID, new { topicId, openid, invite, ip_address, ip_region });
        }
        public void SaveWechatUser(WxUser user)
        {
            if (string.IsNullOrEmpty(user.openid))
                throw new ArgumentException("必须提供用的openid");

            Connection.Execute(SQL_SAVE_USER, user);
        }
        /// <summary>
        /// 获取某篇主题的拉票排行
        /// </summary>
        /// <returns></returns>
        public List<TopicRank> GetTopicInviteRank(string topicId, string userId = "")
        {
            var rank = Connection.Query<TopicRank>(SQL_TOPIC_RANK, new { topicId }).ToList();
            if (!string.IsNullOrEmpty(userId)) {
                if (rank.Any(p => p.openid == userId)) {
                    var me = rank.First(p => p.openid == userId);
                    rank.Remove(me);
                }
                var mine = Connection.QueryFirstOrDefault<TopicRank>(SQL_USER_TOPIC_RANK, new { topicId, userId });
                if (mine == null) {
                    mine = Connection.QueryFirst<TopicRank>(SQL_GET_USER, new { openid = userId });
                }
                rank.Insert(0, mine);
            }
            return rank;
        }
    }
}
