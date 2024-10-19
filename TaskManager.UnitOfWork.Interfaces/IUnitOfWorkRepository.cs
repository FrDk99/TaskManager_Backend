using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Repository.Interfaces;

namespace TaskManager.UnitOfWork.Interfaces
{
    public interface IunitOfWorkRepository
    {
        ILoginRepository LoginRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IConfigurationRepository ConfigurationRepository { get; }
    }
}
