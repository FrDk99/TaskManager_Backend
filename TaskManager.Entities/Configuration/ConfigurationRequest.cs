using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Entities.Configuration
{
    public class ConfigurationRequest
    {
        public string GeneralCode {  get; set; } = string.Empty;
        public string SpecificCode {  get; set; } = string.Empty;
        public string SpecificCodeModification { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public int User {  get; set; }
    }
}
