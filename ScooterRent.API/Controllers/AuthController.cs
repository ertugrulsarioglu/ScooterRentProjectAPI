using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScooterRent.API.Entities;
using ScooterRent.API.Infrastructure.Interfaces;
using ScooterRent.API.ViewModels;

namespace ScooterRent.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> tblUser;
        public AuthController(IRepository<User> tblUser)
        {
            this.tblUser = tblUser;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            try
            {
                var user = tblUser.Get(x => x.Username == model.Username && x.Password == model.Password);
                if (user == null) return await Task.FromResult(Unauthorized("Kullanıcı adı veya şifre hatalı"));
                user.LastLogindDate = DateTime.Now;
                tblUser.Update(user);
                return await Task.FromResult(Ok(user));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }

        }
    }
}
