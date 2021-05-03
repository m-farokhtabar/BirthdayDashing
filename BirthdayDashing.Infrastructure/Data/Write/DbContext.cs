using System.Data;
using System.Data.SqlClient;

namespace BirthdayDashing.Infrastructure.Data.Write
{
    public class DbContext : IDbContext, IDbSet
    {
        private IDbConnection Connection = null;
        private IDbTransaction Transaction = null;
        private readonly string ConnectionString;
        public DbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public IDbConnection GetDbEntities()
        {
            return GetTransactionInstance().Connection;
        }
        public IDbTransaction GetTransactionInstance()
        {
            if (Connection == null || Connection.State != ConnectionState.Open)
            {
                Connection = new SqlConnection(ConnectionString);
                Connection.Open();
            }
            if (Transaction == null)
                Transaction = Connection.BeginTransaction();
            return Transaction;
        }
        private void DisposeTransaction()
        {
            if (Transaction is not null)
            {
                Transaction.Dispose();
                Transaction = null;
            }
        }
        public void DisposeConnection()
        {
            DisposeTransaction();
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Dispose();
            }
        }
    }
}
