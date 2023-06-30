using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScooterRent.API.Entities;
using ScooterRent.API.Enums;
using ScooterRent.API.Infrastructure.Interfaces;

namespace ScooterRent.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IRepository<TransactionHistory> tblTransactionHistory;

        public TransactionController(IRepository<TransactionHistory> tblTransactionHistory)
        {
            this.tblTransactionHistory = tblTransactionHistory;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionHistories([FromQuery] TransactionType? type, [FromQuery] string userId)
        {
            try
            {
                List<TransactionHistory> histories = new();
                if (type.HasValue) histories = tblTransactionHistory.GetList(x => x.TransactionType == type && x.UserId == userId);
                else histories = tblTransactionHistory.GetList(x=>x.UserId == userId);
                histories = histories.OrderByDescending(x => x.CreatedDate).ToList();
                return await Task.FromResult(Ok(histories));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddTransactionHistory([FromBody] TransactionHistory model)
        {
            try
            {
                tblTransactionHistory.Add(model);
               
                return await Task.FromResult(Ok());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
    }
}
