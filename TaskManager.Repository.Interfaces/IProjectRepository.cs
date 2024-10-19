using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entities.Projects;

namespace TaskManager.Repository.Interfaces
{
    public interface IProjectRepository
    {
        Task<bool> CreateProject(ProjectRequest project);
        Task<IEnumerable<ProjectResponse>> GetAll(ProjectRequest project);
        Task<bool> UpdateProject(ProjectRequest project);
        Task<bool> DeleteProject(ProjectRequest project);

    }
}
