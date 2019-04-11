using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoteCore.Components;

namespace VoteCore.Services
{
    public partial class VoteService
    {
        private readonly string SQL_VOTE_RESULT = @"
                select * from Vote_Result r
                inner join Vote_Topic t on t.id = r.id
                inner join Vote_Subject s on s.TopicId = t.id
                inner join Vote_Option o on o.SubjectId = s.Id
                where r.id=@topicId";

        private readonly string SQL_VOTE_DECREASE_OPTION_VOTE = @"
                if exists(select * from Vote_Result where Id=@topicId and SubjectId=@subjectId and OptionId=@optionId)
                begin 
	                if 1<=(select Votes from Vote_Result where Id=@topicId and SubjectId=@subjectId and OptionId=@optionId)
		                delete Vote_Result where Id=@topicId and SubjectId=@subjectId and OptionId=@optionId
	                else
		                update Vote_Result set Votes = Votes - 1 where Id=@topicId and SubjectId=@subjectId and OptionId=@optionId
                end";
        private readonly string SQL_VOTE_INCREASE_OPTION_VOTE = @"
                if not exists(select * from Vote_Result where Id=@topicId and SubjectId=@subjectId and OptionId=@optionId)
	                insert Vote_Result Values(@topicId,@subjectId,@optionId,1)
                else
	                update Vote_Result set Votes = Votes + 1 where Id=@topicId and SubjectId=@subjectId and OptionId=@optionId";
        /// <summary>
        /// 获取投票主题的投票结果
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public List<Result> GetVoteResult(string topicId)
        {
            return Connection.Query<Result, Topic, Subject, Option, Result>(SQL_VOTE_RESULT, (r, t, s, o) => {
                r.Topic = t;
                r.Subject = s;
                r.Option = o;
                return r;
            }, new { topicId }).ToList();
        }
    }
}
