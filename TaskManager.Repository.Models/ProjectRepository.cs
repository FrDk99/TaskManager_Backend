using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entities.Projects;
using TaskManager.Repository.Interfaces;

namespace TaskManager.Repository.Models
{
    public class ProjectRepository : Repository, IProjectRepository
    {

        public ProjectRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            this.sqlConnection = sqlConnection;
            this.sqlTransaction = sqlTransaction;
        }
        public async Task<bool> CreateProject(ProjectRequest project)
        {
            string query = @"INSERT INTO Project (
                projectName, 
                projectDescription, 
                startDate, 
                endDate, 
                status, 
                priority, 
                idManager, 
                budget, 
                userCreation,
                creationDate
            ) 
            VALUES (
                @projectName, 
                @projectDescription, 
                @startDate, 
                @endDate, 
                @status, 
                @priority, 
                @idManager, 
                @budget, 
                @userCreation,
                @creationDate
            )";

            using var cmd = CreateCommand(query);

            cmd.Parameters.AddWithValue("@projectName", project.ProjectName);
            cmd.Parameters.AddWithValue("@projectDescription", project.ProjectDescription);
            cmd.Parameters.AddWithValue("@startDate", project.StartDate);
            cmd.Parameters.AddWithValue("@endDate", project.EndDate);
            cmd.Parameters.AddWithValue("@status", project.Status);
            cmd.Parameters.AddWithValue("@priority", project.Priority);
            cmd.Parameters.AddWithValue("@idManager", project.IdManager);
            cmd.Parameters.AddWithValue("@budget", project.Budget);
            cmd.Parameters.AddWithValue("@userCreation", project.UserCreation);
            cmd.Parameters.AddWithValue("@creationDate", DateTime.Now);

            await cmd.ExecuteNonQueryAsync();
            return true;

        }

        public async Task<bool> DeleteProject(ProjectRequest project)
        {
            string query = @"DELETE FROM Project WHERE @IdProject";

            using var cmd = CreateCommand(query);

            cmd.Parameters.AddWithValue("@IdProject", project.IdProject);

            await cmd.ExecuteNonQueryAsync();

            return true;

        }

        public async Task<IEnumerable<ProjectResponse>> GetAll(ProjectRequest project)
        {

            List<ProjectResponse> projectResponses = new List<ProjectResponse>();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            string query = @"SELECT * FROM Project WHERE 1 = 1";

            if(string.IsNullOrEmpty(project.ProjectName))
            {
                query += " AND projectName LIKE @projectName";
                sqlParameters.Add(new SqlParameter("@projectName", "%" + project.ProjectName + "%"));

            }

            using var cmd = CreateCommand(query);

            cmd.Parameters.AddRange(sqlParameters.ToArray());
            
            using(var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync()) 
                {
                    ProjectResponse projectResponse = new ProjectResponse
                    {
                        IdProject = reader.GetInt32(reader.GetOrdinal("IdProject")),
                        ProjectName = reader.GetString(reader.GetOrdinal("projectName")),
                        ProjectDescription = reader.GetString(reader.GetOrdinal("projectDescription")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("startDate")),
                        EndDate = reader.GetDateTime(reader.GetOrdinal("endDate")),
                        Status = reader.GetString(reader.GetOrdinal("status")),
                        Priority = reader.GetString(reader.GetOrdinal("priority")),
                        IdManager = reader.GetInt32(reader.GetOrdinal("idManager")),
                        Budget = reader.GetDecimal(reader.GetOrdinal("budget")),
                        UserCreation = reader.GetInt32(reader.GetOrdinal("userCreation")),
                        CreationDate = reader.GetDateTime(reader.GetOrdinal("creationDate")),
                        ModificationDate = reader.GetDateTime(reader.GetOrdinal("modificationDate")),
                        IsArchived = reader.GetBoolean(reader.GetOrdinal("isArchived"))
                    };

                    projectResponses.Add(projectResponse);
                }
            }

            return projectResponses;


        }

        public async Task<bool> UpdateProject(ProjectRequest project)
        {
            string query = @"UPDATE FROM Project SET
                projectName = @projectName, 
                projectDescription = @projectDescription, 
                startDate = @startDate, 
                endDate = @endDate, 
                status = @status, 
                priority = @priority, 
                idManager = @idManager, 
                budget = @budget, 
                userModification = @userModification,
                modificationDate = @modificationDate
                WHERE idProject = @idProject
            ";

            using var cmd = CreateCommand(query);

            cmd.Parameters.AddWithValue("@projectName", project.ProjectName);
            cmd.Parameters.AddWithValue("@projectDescription", project.ProjectDescription);
            cmd.Parameters.AddWithValue("@startDate", project.StartDate);
            cmd.Parameters.AddWithValue("@endDate", project.EndDate);
            cmd.Parameters.AddWithValue("@status", project.Status);
            cmd.Parameters.AddWithValue("@priority", project.Priority);
            cmd.Parameters.AddWithValue("@idManager", project.IdManager);
            cmd.Parameters.AddWithValue("@budget", project.Budget);
            cmd.Parameters.AddWithValue("@userCreation", project.UserCreation);
            cmd.Parameters.AddWithValue("@creationDate", DateTime.Now);

            await cmd.ExecuteNonQueryAsync();
            return true;
        }
    }
}
