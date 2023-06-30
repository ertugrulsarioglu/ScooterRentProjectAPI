using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScooterRent.API.Entities;
using ScooterRent.API.Enums;
using ScooterRent.API.Infrastructure.Interfaces;

namespace ScooterRent.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RemainderController : ControllerBase
    {
        private readonly IRepository<Remainder> tblRemainder;
        private readonly IRepository<TransactionHistory> tblTransactionHistory;

        public RemainderController(IRepository<Remainder> tblRemainder, IRepository<TransactionHistory> tblTransactionHistory)
        {
            this.tblRemainder = tblRemainder;
            this.tblTransactionHistory = tblTransactionHistory;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRemainder([FromQuery] string userId)
        {
            try
            {
                decimal entranceTotal = 0, exitTotal = 0;
                var result = tblRemainder.GetList(x => x.UserId == userId).GroupBy(x => x.RemainderType).Select(g => new { remainderType = g.Key, sum = g.Sum(x => x.Amount) });
                var entrance = result.FirstOrDefault(x => x.remainderType == RemainderType.Entrance);
                if (entrance is not null) entranceTotal = entrance.sum;
                var exit = result.FirstOrDefault(x => x.remainderType == RemainderType.Exit);
                if (exit is not null) exitTotal = exit.sum;

                return await Task.FromResult(Ok(entranceTotal - exitTotal));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddRemainder([FromBody] Remainder model)
        {
            try
            {
                tblRemainder.Add(model);
                tblTransactionHistory.Add(new()
                {
                    TransactionType = TransactionType.Remainder,
                    CreatedDate = DateTime.Now,
                    UserId = model.UserId,
                    Description = $"Bakiye yükleme : {model.Amount} ₺"
                });
                return await Task.FromResult(Ok());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
    }
}
