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
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Ingrese valores."); // Esto será manejado por el middleware
            }

            var resp = await _loginDomain.Login(user);

            if (resp == null)
            {
                throw new Exception("Usuario no se encuentra en la base de datos."); // También será manejado por el middleware
            }

            // Si todo es correcto, retorna OK
            return Ok(resp);
        }

    }
}
