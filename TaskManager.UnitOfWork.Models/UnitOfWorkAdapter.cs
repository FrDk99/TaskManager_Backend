using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.UnitOfWork.Interfaces;

namespace TaskManager.UnitOfWork.Models
{
    public class UnitOfWorkAdapter : IUnitOfWorkAdapter
    {

        private SqlConnection _connection { get; set; }
        private SqlTransaction _transaction { get; set; }

        public IunitOfWorkRepository Repositories { get; set; }


        public UnitOfWorkAdapter(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();

            Repositories = new UnitOfWorkRepository(_connection, _transaction); //Instanciamos los repositorios
        }

        public void Dispose() //Liberamos los recursos
        {
            if(_transaction != null)
            {
                _transaction.Dispose();
            }

            if(_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }

            Repositories = null;

        }

        public void SaveChanges() //Confirmamos la transaccion
        {
            _transaction.Commit();
        }
    }
}
