using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Repository.Interfaces;
using TaskManager.Repository.Models;
using TaskManager.UnitOfWork.Interfaces;

namespace TaskManager.UnitOfWork.Models
{
    public class UnitOfWorkRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction) : IunitOfWorkRepository
    {
        public ILoginRepository LoginRepository { get; } = new LoginRepository(sqlConnection, sqlTransaction);
        public IProjectRepository ProjectRepository { get; } = new ProjectRepository(sqlConnection, sqlTransaction);
        public IConfigurationRepository ConfigurationRepository { get; } = new ConfigurationRepository(sqlConnection, sqlTransaction);
    }
}
