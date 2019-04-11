using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoteCore.Components;
using VoteCore.Models;

namespace VoteCore.Services
{
    public partial class VoteService
    {
        private readonly string SQL_TOPIC_VOTES = @"
                select * from Vote_Pool p
                inner join Vote_Topic t on p.TopicId = t.Id
                inner join Vote_Subject s on p.SubjectId = s.Id
                left join Vote_Option o on p.OptionId = o.Id
                where t.Id = @topicId";
        private readonly string SQL_TOPIC_LIST = @"
                select * from Vote_Topic t
                inner join Vote_Subject s on s.TopicId = t.id and s.[Status] = 1
                inner join Vote_Option o on o.SubjectId = s.id and o.[Status] = 1
                left join Vote_Orgnization g on o.Reference = g.id and o.ReferenceType = 2 and g.[Status] = 1
                left join Vote_Competitor c on o.Reference = c.Id and o.ReferenceType = 1 and c.[Status] = 1
                left join Vote_Result r on r.id=t.id and r.SubjectId=s.Id and r.OptionId = o.id";
        private readonly string SQL_TOPIC_VOTE = @"insert Vote_Pool(TopicId,SubjectId,OptionId,VoteUser,VoteOrder) Values(@TopicId,@SubjectId,@OptionId,@VoteUser,@VoteOrder)";
        private readonly string SQL_TOPIC_REMOVE_VOTES = @"delete Vote_Pool where VoteUser=@voteUser and TopicId=@topicId";
        private readonly string SQL_TOPIC_LIKE = @"update Vote_Topic set Likes = Likes + 1 where Id= @topicId;select Likes from Vote_Topic where Id=@topicId";
        /// <summary>
        /// 获取所有话题
        /// </summary>
        /// <param name="topicId">话题编号</param>
        /// <returns></returns>
        public List<Topic> GetTopics()
        {
            Connection.Query<Topic>(SQL_TOPIC_LIST, new Type[6] { typeof(Topic), typeof(Subject), typeof(Option), typeof(Orgnization), typeof(Competitor), typeof(Result) }, map: topicMapper);
            return topics;
        }

        public Topic GetTopic(string topicId)
        {
            string sqlSingleTopic = SQL_TOPIC_LIST + " where t.id=@topicId";
            Connection.Query<Topic>(sqlSingleTopic, new Type[6] { typeof(Topic), typeof(Subject), typeof(Option), typeof(Orgnization), typeof(Competitor), typeof(Result) }, topicMapper, new { topicId });
            return topics.Single();
        }

        private List<Topic> topics = new List<Topic>();
        private Topic topicMapper(object[] args)
        {
            var t = args[0] as Topic;
            var s = args[1] as Subject;
            var o = args[2] as Option;
            var g = args[3] as Orgnization;
            var c = args[4] as Competitor;
            var r = args[5] as Result;
            o.Votes = r.Votes;
            if (o.ReferenceType == VoteReferenceType.Competitor)
                o.Reference = c;
            else if (o.ReferenceType == VoteReferenceType.Orgnization)
                o.Reference = g;
            else
                o.Reference = null;
            Topic topic;
            Subject sub;

            if (topics.Any(p => p.Id == t.Id)) {
                topic = topics.Single(p => p.Id == t.Id);
            }
            else {
                topic = t;
                topic.Subjects = new List<Subject>();
                topics.Add(topic);
            }

            if (topic.Subjects.Any(p => p.Id == s.Id)) {
                sub = topic.Subjects.Single(p => p.Id == s.Id);
                sub.Options.Add(o);
            }
            else {
                sub = s;
                sub.Options = new List<Option>();
                sub.Options.Add(o);
                topic.Subjects.Add(sub);
            }
            return topic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="votes"></param>
        public List<Vote> Vote(string topicId, VotePost votes)
        {
            // 获取这个人所有的投票
            // 清除原来的票
            // 投新票

            if (string.IsNullOrEmpty(votes.uid))
                throw new ArgumentNullException("wechat id");

            var userVotes = GetUserVotes(topicId, votes.uid);

            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();

            var trans = Connection.BeginTransaction();
            try {
                #region - 清除voteUser在主题topicId下的所有投票结果
                foreach (var vote in userVotes)
                    Connection.Execute(SQL_VOTE_DECREASE_OPTION_VOTE, new { topicId = vote.Topic.Id, subjectId = vote.Subject.Id, optionId = vote.Option.Id }, trans);
                Connection.Execute(SQL_TOPIC_REMOVE_VOTES, new { topicId, voteUser = votes.uid }, trans);
                userVotes.Clear();
                #endregion

                for (var i = 0; i < votes.subs.Count(); i++) {
                    var subject = votes.subs[i];
                    for (var j = 0; j < subject.ops.Count(); j++) {
                        var option = subject.ops[j];
                        if (option.order <= 0) option.order = Option.DefaultOrder;
                        Connection.Execute(SQL_TOPIC_VOTE, new { TopicId = topicId, SubjectId = subject.id, OptionId = option.id, VoteUser = votes.uid, VoteOrder = option.order }, trans);
                        Connection.Execute(SQL_VOTE_INCREASE_OPTION_VOTE, new { topicId, subjectId = subject.id, optionId = option.id }, trans);
                    }
                }
                trans.Commit();
                return userVotes;
            }
            catch (Exception ex) {
                trans.Rollback();
                throw ex;
            }
            finally {
                trans.Dispose();
                Connection.Close();
            }
        }

        public int Like(string topicId)
        {
            return Connection.ExecuteScalar<int>(SQL_TOPIC_LIKE, new { topicId });
        }
        /// <summary>
        /// 获取指定主题的所有票
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public List<Vote> GetTopicVotes(string topicId)
        {
            return Connection.Query<Vote, Topic, Subject, Option, Vote>(SQL_TOPIC_VOTES, (v, t, s, o) => {
                v.Option = o;
                v.Subject = s;
                v.Topic = t;
                return v;
            }, new { topicId }).ToList();
        }
    }
}
