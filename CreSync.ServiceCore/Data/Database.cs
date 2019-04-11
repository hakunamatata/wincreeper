using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreSync.ServiceCore.Data
{
    public static class Database
    {
        private static IDbConnection connection;
        public static IDbConnection Connection
        {
            get
            {
                if (connection == null)
                    connection = new SqlConnection(AppSetting.Current.ConnectionString);
                return connection;
            }
        }


    }
}
