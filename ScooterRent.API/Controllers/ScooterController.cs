using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScooterRent.API.Entities;
using ScooterRent.API.Infrastructure.Interfaces;

namespace ScooterRent.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScooterController : ControllerBase
    {
        private readonly IRepository<Scooter> tblScooter;

        public ScooterController(IRepository<Scooter> tblScooter)
        {
            this.tblScooter = tblScooter;
        }
        [HttpGet]
        public async Task<IActionResult> GetScooters()
        {
            try
            {
                var result = tblScooter.GetList();
                return await Task.FromResult(Ok(result));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddScooter([FromBody] Scooter model)
        {
            try
            {
                tblScooter.Add(model);
                return await Task.FromResult(Ok());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateScooter([FromBody] Scooter model)
        {
            try
            {
                tblScooter.Update(model);
                return await Task.FromResult(Ok());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveScooter([FromQuery] string id)
        {
            try
            {
                tblScooter.Remove(id);
                return await Task.FromResult(Ok());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
    }
}
