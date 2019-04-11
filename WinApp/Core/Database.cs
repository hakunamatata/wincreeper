using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Core
{
    public static class Database
    {
        private static IDbConnection connection = null;
        public static IDbConnection Connection
        {
            get
            {
                if (connection == null)
                    connection = new SqlConnection(AppConfiguration.Current.ConnectionString);
                return connection;
            }
        }
    }
}
