using BirthdayDashing.Application.Configuration.Data;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BirthdayDashing.Infrastructure.Data.Read
{
    public class ReadDbSet : IReadDbSet, IDisposable
    {
        private IDbConnection Connection = null;
        private readonly string ConnectionString;
        public ReadDbSet(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDbConnection GetDbEntities()
        {
            if (Connection is null || Connection.State != ConnectionState.Open)
            {
                Connection = new SqlConnection(ConnectionString);
                Connection.Open();
            }
            return Connection;
        }

        public void Dispose()
        {
            if (Connection is not null)
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Dispose();
            }
        }
    }
}
