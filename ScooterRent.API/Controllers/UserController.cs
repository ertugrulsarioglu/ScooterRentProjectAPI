using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ScooterRent.API.Entities;
using ScooterRent.API.Infrastructure.Interfaces;

namespace ScooterRent.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> tblUser;

        public UserController(IRepository<User> tblUser)
        {
            this.tblUser = tblUser;
        }
        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var result = tblUser.GetList(x=>x.Email == email);
                if(!result.Any()) return BadRequest();
                return await Task.FromResult(Ok(result.First()));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest());
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = tblUser.GetList();
                return await Task.FromResult(Ok(result));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User model)
        {
            try
            {
                tblUser.Add(model);
                return await Task.FromResult(Ok());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] User model)
        {
            try
            {
                var result = tblUser.Update(model);
                return await Task.FromResult(Ok(result));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveUser([FromQuery] string id)
        {
            try
            {
                tblUser.Remove(id);
                return await Task.FromResult(Ok());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }

    }
}
