using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Interfaces;
using TaskManager.Entities.Configuration;
using TaskManager.UnitOfWork.Interfaces;

namespace TaskManager.Domain.Models
{
    public class ConfigurationDomain : IConfigurationDomain
    {

        private readonly IUnitOfWork _unitOfWork;

        public ConfigurationDomain(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateConfiguration(ConfigurationRequest configuration)
        {
            var context = _unitOfWork.Create(); 
            
            var result = await context.Repositories.ConfigurationRepository.CreateConfiguration(configuration);

            context.SaveChanges();
            context.Dispose();

            return result;
            
        }

        public async Task<bool> DeleteConfiguration(ConfigurationRequest configuration)
        {
            var context = _unitOfWork.Create();

            var result = await context.Repositories.ConfigurationRepository.DeleteConfiguration(configuration);

            context.SaveChanges();
            context.Dispose();

            return result;
        }

        public async Task<IEnumerable<ConfigurationResponse>> GetAll(ConfigurationRequest request)
        {
            var context = _unitOfWork.Create();

            var lista = await context.Repositories.ConfigurationRepository.GetAll(request);

            return lista;

        }

        public async Task<bool> UpdateConfiguration(ConfigurationRequest configuration)
        {
            var context = _unitOfWork.Create();

            var result = await context.Repositories.ConfigurationRepository.UpdateConfiguration(configuration);

            context.SaveChanges();
            context.Dispose();

            return result;
        }
    }
}
