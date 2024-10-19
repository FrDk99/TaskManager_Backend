using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Domain.Interfaces;
using TaskManager.Entities.Configuration;

namespace TaskManager_backend.Controllers
{

    [ApiController]
    [Route("api/v1/config/[action]")]
    [Authorize]
    public class ConfigurationController : Controller
    {
        private readonly IConfigurationDomain _configurationDomain;

        public ConfigurationController(IConfigurationDomain configurationDomain)
        {
            _configurationDomain = configurationDomain;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ConfigurationRequest request)
        {

            var result = await _configurationDomain.GetAll(request);
            return Ok(result);

        }

        [HttpPost]
        public async Task<IActionResult> CreateConfiguration([FromBody] ConfigurationRequest request)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.Name);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("No se pudo obtener el ID del usuario.");
            }

            request.User = userId;

            var result = await _configurationDomain.CreateConfiguration(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateConfiguration([FromBody] ConfigurationRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.Name);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("No se pudo obtener el ID del usuario.");
            }

            request.User = userId;
            var result = await _configurationDomain.UpdateConfiguration(request);
            return Ok(result);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteConfiguration([FromBody] ConfigurationRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.Name);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("No se pudo obtener el ID del usuario.");
            }

            request.User = userId;

            var result = await _configurationDomain.DeleteConfiguration(request);
            return Ok(result);
        }



    }


}
