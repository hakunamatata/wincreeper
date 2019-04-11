using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteCore.Services
{
    public abstract class BaseRepository
    {
        public IDbConnection Connection {get; set;} = Database.Connection;

    }
}
