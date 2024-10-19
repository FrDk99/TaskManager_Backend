using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entities.Configuration;
using TaskManager.Helpers;
using TaskManager.Repository.Interfaces;

namespace TaskManager.Repository.Models
{
    public class ConfigurationRepository : Repository , IConfigurationRepository
    {

        public ConfigurationRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            this.sqlConnection = sqlConnection;
            this.sqlTransaction = sqlTransaction;
        }

        public async Task<bool> CreateConfiguration(ConfigurationRequest configuration)
        {
            string query = @"INSERT INTO Configuration (generalCode, specificCode, description, userCreation, creationDate) VALUES (@generalCode, @specificCode, @description, @userCreation, @creationDate)";

            using var cmd = CreateCommand(query);
            cmd.Parameters.AddWithValue("@generalCode", configuration.GeneralCode);
            cmd.Parameters.AddWithValue("@specificCode", configuration.SpecificCode);
            cmd.Parameters.AddWithValue("@description", configuration.Description);
            cmd.Parameters.AddWithValue("@userCreation", configuration.User);
            cmd.Parameters.AddWithValue("@creationDate", DateTime.Now);
            await cmd.ExecuteNonQueryAsync();
            return true;
        
        }

        public async Task<bool> DeleteConfiguration(ConfigurationRequest configuration)
        {

            string query = @"DELETE FROM Configuration WHERE generalCode = @generalCode AND specificCode = @specificCode";

            using var cmd = CreateCommand(query);

            cmd.Parameters.AddWithValue("@generalCode", configuration.GeneralCode);
            cmd.Parameters.AddWithValue("@specificCode", configuration.SpecificCode);

            await cmd.ExecuteNonQueryAsync();

            return true;


        }

        public async Task<IEnumerable<ConfigurationResponse>> GetAll(ConfigurationRequest request)
        {
            List<ConfigurationResponse> lista = [];
            string query = @"SELECT generalCode, specificCode, description FROM Configuration WHERE generalCode LIKE @generalCode AND specificCode LIKE @specificCode";

            using var cmd = CreateCommand(query);

            cmd.Parameters.AddWithValue("@generalCode", "%" + request.GeneralCode + "%");
            cmd.Parameters.AddWithValue("@specificCode", "%" + request.SpecificCode + "%");

            using(var reader = await cmd.ExecuteReaderAsync())
            {

                while (await reader.ReadAsync()) 
                {

                    lista.Add(new ConfigurationResponse
                    {
                        GeneralCode = ConvertHelper.NullToString(reader["generalCode"]),
                        SpecificCode = ConvertHelper.NullToString(reader["specificCode"]),
                        Description = ConvertHelper.NullToString(reader["description"]),
                    });
                }
            }

            return lista;

        }

        public async Task<bool> UpdateConfiguration(ConfigurationRequest configuration)
        {
            string query = @"UPDATE FROM Configuration SET 
                    specificCode = @specificCodeModification, 
                    description = @description, 
                    userModification = @userModification , 
                    modificationDate = @modificationDate
                    WHERE generalCode = @generalCode AND specificCode = @specificCode";

            using var cmd = CreateCommand(query);
            cmd.Parameters.AddWithValue("@generalCode", configuration.GeneralCode);
            cmd.Parameters.AddWithValue("@specificCodeModification", configuration.SpecificCodeModification);
            cmd.Parameters.AddWithValue("@specificCode", configuration.SpecificCode);
            cmd.Parameters.AddWithValue("@description", configuration.Description);
            cmd.Parameters.AddWithValue("@userModification", configuration.User);
            cmd.Parameters.AddWithValue("@modificationDate", DateTime.Now);
            await cmd.ExecuteNonQueryAsync();
            return true;
        }
    }
}
