using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Util;
using VoteCore.Components;
using VoteCore.Models;
using VoteCore.Services;

/// <summary>
/// POST https://mini.artibition.cn/vote/auth
/// GET  https://mini.artibition.cn/vote/topic/{id} -- 获取投票主题
/// POST https://mini.artibition.cn/vote/topic/{id} -- 投票动作
/// {
///     uid: 微信用户的OpenId
///     subs: [
///         {
///             id: String
///             ops: [{
///                 id: String,
///                 order:1
///             }]
///         }
///     ]
/// }
/// 
/// POST https://mini.artibition.cn/vote/topic/like -- 喜欢主题
/// {
///     tid : String 主题编号
/// }
/// </summary>
namespace AustraliaVote.Controllers
{
    public class VoteController : ApiController
    {
        static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private VoteService voteService;
        private WxUserService userService;

        public IHttpActionResult Options()
        {
            return Ok();
        }

        [Route("vote/topic/{id}")]
        [HttpGet]
        public IHttpActionResult GetTopic(string id, string uid)
        {
            try {
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentNullException("id");

                voteService = new VoteService();
                var topic = voteService.GetTopic(id);
                if (!string.IsNullOrEmpty(uid)) {
                    var myVotes = voteService.GetUserVotes(id, uid);
                    var myOptions = myVotes.Select(p => p.Option.Id);
                    var subs = myVotes.Select(p => p.Subject).Select(p => new { id = p.Id, ops = new List<object>() }).ToList();
                    subs.ForEach(p => {
                        foreach (var v in myVotes) {
                            if (v.Subject.Id == p.id)
                                p.ops.Add(new { id = v.Option.Id, order = v.Option.Order });
                        }
                    });
                    return Ok(topic.Merge(new {
                        MyVotes = myOptions,
                        MySubs = subs
                    }));
                }

                return Ok(topic);
            }
            catch (ArgumentNullException ex_null) {
                log.Error(ex_null);
                return InternalServerError(ex_null);
            }
            catch (Exception ex) {
                log.Error(ex);
                return InternalServerError(ex);
            }
        }

        [Route("vote/topic/{id}")]
        [HttpPost]
        public IHttpActionResult Vote(string id, [FromBody] VotePost voteBody)
        {
            try {
                voteService = new VoteService();
                return Ok(voteService.Vote(id, voteBody));
            }
            catch (Exception ex) {
                log.Error(ex);
                return InternalServerError(ex);
            }
        }

        [Route("vote/topic/myvotes")]
        [HttpPost]
        public IHttpActionResult MyVotes([FromBody] RankPost post)
        {
            try {
                voteService = new VoteService();
                return Ok();
            }
            catch (Exception ex) {
                log.Error(ex);
                return InternalServerError(ex);
            }
        }


        [Route("vote/topic/like")]
        [HttpPost]
        public IHttpActionResult Like([FromBody] LikePost likeBody)
        {
            try {
                voteService = new VoteService();
                return Ok(voteService.Like(likeBody.tid));
            }
            catch (Exception ex) {
                log.Error(ex);
                return InternalServerError(ex);
            }
        }

        [Route("vote/topic/rank")]
        [HttpPost]
        public IHttpActionResult GetRank([FromBody] RankPost rankBody)
        {
            try {
                userService = new WxUserService();
                if (string.IsNullOrEmpty(rankBody.tid))
                    throw new ArgumentNullException("tid");

                List<TopicRank> rank = Cache.GetCache<List<TopicRank>>("topicRank");
                if (rank == null) {
                    rank = userService.GetTopicInviteRank(rankBody.tid, rankBody.uid);
                    Cache.WriteCache("topicRank", rank, DateTime.Now.AddMinutes(5));
                }
                return Ok(rank);
            }
            catch (Exception ex) {
                log.Error(ex);
                return InternalServerError(ex);
            }
        }
    }
}
