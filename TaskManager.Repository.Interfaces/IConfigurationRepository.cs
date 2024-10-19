using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entities.Configuration;

namespace TaskManager.Repository.Interfaces
{
    public interface IConfigurationRepository
    {
        Task<IEnumerable<ConfigurationResponse>> GetAll(ConfigurationRequest request);
        Task<bool> CreateConfiguration(ConfigurationRequest configuration);
        Task<bool> UpdateConfiguration(ConfigurationRequest configuration);
        Task<bool> DeleteConfiguration(ConfigurationRequest configuration);
    }
}
