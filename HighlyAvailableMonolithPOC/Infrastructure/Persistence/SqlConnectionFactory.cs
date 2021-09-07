using System;
using System.Data;
using System.Data.SqlClient;

namespace HighlyAvailableMonolithPOC.Infrastructure.Persistence
{
    public class SqlConnectionFactory : IDisposable
    {
        private readonly string connectionString;
        private IDbConnection connection;

        public SqlConnectionFactory(string connectionString)
        { 
            this.connectionString = connectionString;
        }


        public IDbConnection GetOpenConnection()
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }

            return connection;
        }

        public void Dispose()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Dispose();
            }
        }
    }
}
