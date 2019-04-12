using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace VoteCore.Services
{
    public static class Database
    {
        //private static IDbConnection connection;
        public static IDbConnection Connection
        {
            get
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings["VoteDb"].ConnectionString);
                //if (connection == null)
                //    connection = new SqlConnection(ConfigurationManager.ConnectionStrings["VoteDb"].ConnectionString);
                //return connection;
            }
        }

    }
}