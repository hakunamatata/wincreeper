using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        private VoteService topicService;


        [Route("vote/topic/{id}")]
        [HttpGet]
        public IHttpActionResult GetTopic(string id)
        {
            try {
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentNullException("id");

                topicService = new VoteService();
                return Ok(topicService.GetTopic(id));
            }
            catch (ArgumentNullException ex_null) {
                return InternalServerError(ex_null);
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }
        }

        [Route("vote/topic/{id}")]
        [HttpPost]
        public IHttpActionResult Vote(string id, [FromBody] VotePost voteBody)
        {
            try {
                topicService = new VoteService();
                return Ok(topicService.Vote(id, voteBody));
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }
        }



        [Route("vote/topic/like")]
        [HttpPost]
        public IHttpActionResult Like([FromBody] LikePost likeBody)
        {
            try {
                topicService = new VoteService();
                return Ok(topicService.Like(likeBody.tid));
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }
        }
    }
}
