using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.UnitOfWork.Interfaces;

namespace TaskManager.UnitOfWork.Models
{
    public class UnitOfWorkSQL : IUnitOfWork
    {

        private readonly string _connectionString;
        private IConfiguration _configuration;

        public UnitOfWorkSQL(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionStrings").GetSection("MiConexion").Value;
        }

        public IUnitOfWorkAdapter Create()
        {
            return new UnitOfWorkAdapter(_connectionString);
        }
    }
}
