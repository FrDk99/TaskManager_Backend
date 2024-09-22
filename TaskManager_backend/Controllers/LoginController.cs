using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Interfaces;
using TaskManager.Entities.Users;

namespace TaskManager_backend.Controllers
{

    [ApiController]
    [Route("api/v1/login/[action]")]
    public class LoginController : Controller
    {

        private readonly ILoginDomain _loginDomain;

        public LoginController(ILoginDomain loginDomain)
        {
            _loginDomain = loginDomain;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserRequest user)
        {

            if(user == null)
            {
                return BadRequest("Ingrese valores.");
            }

            UserResponse? resp = await _loginDomain.Login(user);

            if(resp == null)
            {
                return BadRequest("Usuario no se encuentra en la base de datos.");
            }

            return Ok(resp);
        }

    }
}
