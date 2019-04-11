using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoteCore.Components;
using Dapper;
using CreSync.ServiceCore.Data;
using VoteCore.Models;

namespace VoteCore.Services
{
    public partial class VoteService
    {
        private readonly string SQL_VOTE_GET_BY_USER = @"select * from Vote_Pool  p
                inner join Vote_Topic t on p.TopicId = t.id
                inner join Vote_Subject s on p.SubjectId = s.id
                inner join Vote_Option o on p.OptionId = o.id
                where p.TopicId=@topicId and p.VoteUser=@userId";

        public List<Vote> GetUserVotes(string topicId, string userId)
        {
            return Connection.Query<Vote, Topic, Subject, Option, Vote>(SQL_VOTE_GET_BY_USER, (p, t, s, o) => {
                p.Topic = t;
                p.Subject = s;
                p.Option = o;
                return p;
            }, new { topicId, userId }).ToList();

        }
    }
}
