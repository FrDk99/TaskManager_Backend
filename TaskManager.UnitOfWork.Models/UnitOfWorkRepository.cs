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
    public class UnitOfWorkRepository : IunitOfWorkRepository
    {
        public ILoginRepository LoginRepository { get; }


        public UnitOfWorkRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            LoginRepository = new LoginRepository(sqlConnection, sqlTransaction);
        }

    }
}
