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
                left join Wx_UserRelation r on p.VoteUser = r.UserId
                left join Wx_User u on r.FromUserId = u.openid
                where p.TopicId = @topicId
                group by u.openid,u.nickname,u.headimgurl) as t
                order by t.[rank] desc";

        private readonly string SQL_USER_TOPIC_RANK = @"
                select top 10 * from (
                select u.openid,u.nickname,u.headimgurl,count(p.Id) [rank]
                from [Vote].[dbo].[Vote_Pool] p
                left join Wx_UserRelation r on p.VoteUser = r.UserId
                left join Wx_User u on r.FromUserId = u.openid
                where p.TopicId = @topicId and r.FromUserId=@userId
                group by u.openid,u.nickname,u.headimgurl) as t
                order by t.[rank] desc";
        private readonly string SQL_GET_USER = @"select * from Wx_User where openid=@openid";


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
