using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Interfaces;
using TaskManager.Entities.Projects;
using TaskManager.UnitOfWork.Interfaces;

namespace TaskManager.Domain.Models
{
    public class ProjectDomain : IProjectDomain
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProjectDomain(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateProject(ProjectRequest project)
        {

            var context = _unitOfWork.Create();
           
            bool result = await context.Repositories.ProjectRepository.CreateProject(project);

            if(result == true)
            {
                context.SaveChanges();
                context.Dispose();
                return true;

            } else
            {
                return false;
            }
        
        }

        public Task<bool> DeleteProject(ProjectRequest project)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectResponse>> GetAll(ProjectRequest project)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProject(ProjectRequest project)
        {
            throw new NotImplementedException();
        }
    }
}
