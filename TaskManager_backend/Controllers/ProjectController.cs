using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Domain.Interfaces;
using TaskManager.Entities.Projects;

namespace TaskManager_backend.Controllers
{

    [ApiController]
    [Route("api/v1/project/[action]")]
    [Authorize]
    public class ProjectController : Controller
    {

        private readonly IProjectDomain _projectDomain;
        public ProjectController(IProjectDomain projectDomain)
        {
            _projectDomain = projectDomain;
        }

        //    [HttpGet]
        //    public async Task<IActionResult> GetAll([FromQuery] ProjectRequest project)
        //    {
        //        return View();
        //    }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectRequest project)
        {
            if (project == null)
            {
                return BadRequest("El proyecto no puede ser nulo.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.Name);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("No se pudo obtener el ID del usuario.");
            }

            project.UserCreation = userId;

            var result = await _projectDomain.CreateProject(project);

            if (result)
            {
                var projectResponse = new ProjectResponse
                {
                    IdProject = project.IdProject,
                    ProjectName = project.ProjectName
                };

                return Ok(projectResponse);
            }

            return BadRequest("No se pudo crear el proyecto. Verifica los datos ingresados.");
        }




    }
}
