using System.Data;
using System.Data.SqlClient;
using TaskManager.Entities.Users;
using TaskManager.Helpers;
using TaskManager.Repository.Interfaces;

namespace TaskManager.Repository.Models
{
    public class LoginRepository : Repository, ILoginRepository
    {

        public LoginRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            this.sqlConnection = sqlConnection;
            this.sqlTransaction = sqlTransaction;
        }

        public async Task<UserResponse?> Login(UserRequest usuario)
        {

            string query = @"SELECT TOP 1 Id, Email, Password FROM Users WHERE Email = @email AND Password = @password";
            UserResponse? response = null;

            try
            {
                using var cn = CreateCommand(query);
                cn.Parameters.Add("@email", SqlDbType.VarChar).Value = usuario.Email;
                cn.Parameters.Add("@password", SqlDbType.VarChar).Value = usuario.Password;

                using var reader = await cn.ExecuteReaderAsync();
                while(await reader.ReadAsync())
                {
                    response = new UserResponse
                    {
                        Id = reader.GetInt32("Id"),
                        Email = ConvertHelper.NullToString(reader["Email"]),
                        Password = ConvertHelper.NullToString(reader["Password"]),
                    };
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
        }
    }
}
