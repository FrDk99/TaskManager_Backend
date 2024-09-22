using System.Data.SqlClient;

namespace TaskManager.Repository.Models
{
    public class Repository
    {

        protected SqlConnection? sqlConnection;
        protected SqlTransaction? sqlTransaction;

        protected SqlCommand CreateCommand(string query) // Creamos un comand para consultas en un bd de SQL Server
        {
            return new SqlCommand(query, sqlConnection, sqlTransaction);
        }

    }
}
