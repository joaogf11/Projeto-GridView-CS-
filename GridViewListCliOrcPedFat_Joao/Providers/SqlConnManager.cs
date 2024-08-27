using System;
using System.Data.SqlClient;

namespace WindowsFormsGridView.GridViewListCliOrcPedFat_Joao.Providers
{
    public class SqlConnManager : IDisposable
    {
        private readonly string _connectionString =
            "Server=NBJOAO;Database=DBDEV;User ID=sa;Password=dp;MultipleActiveResultSets=True;";

        private SqlConnection _connection;

        public SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }

            return _connection;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}