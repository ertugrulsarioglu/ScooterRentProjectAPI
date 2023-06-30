using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScooterRent.API.Entities;
using ScooterRent.API.Enums;
using ScooterRent.API.Infrastructure.Interfaces;

namespace RentDetailRent.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RentDetailController : ControllerBase
    {
        private readonly IRepository<RentDetail> tblRentDetail;
        private readonly IRepository<TransactionHistory> tblTransactionHistory;
        private readonly IRepository<Remainder> tblRemainder;
        private readonly IRepository<Scooter> tblScooter;

        public RentDetailController(IRepository<RentDetail> tblRentDetail, IRepository<TransactionHistory> tblTransactionHistory, IRepository<Remainder> tblRemainder, IRepository<Scooter> tblScooter)
        {
            this.tblRentDetail = tblRentDetail;
            this.tblTransactionHistory = tblTransactionHistory;
            this.tblRemainder = tblRemainder;
            this.tblScooter = tblScooter;
        }

        [HttpGet]
        public async Task<IActionResult> GetRentDetails()
        {
            try
            {
                var result = tblRentDetail.GetList();
                return await Task.FromResult(Ok(result));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddRentDetail([FromBody] RentDetail model)
        {
            try
            {
                var result = tblRentDetail.Add(model);
                var scooter = tblScooter.Get(x => x.Id == model.ScooterId);
                if (scooter.State == ScooterState.InUse) return await Task.FromResult(BadRequest("Bu scooter kullanımdadır."));
                scooter.State = ScooterState.InUse;
                tblScooter.Update(scooter);
                return await Task.FromResult(Ok(result));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRentDetail([FromBody] RentDetail model)
        {
            try
            {
                TimeSpan difference = model.EndDate - model.StartDate;
                var scooter = tblScooter.Get(x=>x.Id == model.ScooterId);
                var total = (decimal)difference.TotalMinutes * scooter.RentPrice;
                var remainder = GetUserRemainder(model.UserId);
                if(remainder < total) return await Task.FromResult(BadRequest("Yetersiz bakiye!\nLütfen bakiye yükleyip daha sonra tekrar deneyiniz."));

                var result = tblRentDetail.Update(model);
                tblRemainder.Add(new()
                {
                    Amount = total,
                    RemainderType = RemainderType.Exit,
                    UserId = result.UserId
                });
                tblTransactionHistory.Add(new()
                {
                    TransactionType = TransactionType.ScooterRent,
                    CreatedDate = DateTime.Now,
                    UserId = result.UserId,
                    Description = $"Scooter Adı : {scooter.Name}\nToplam kullanım zamanı:{difference.ToString(@"m\:ss")} dk.\nScooter kiralama bedeli: {total.ToString("C2").Replace("₺","")} ₺"
                });
                scooter.State = ScooterState.NotInUse;
                scooter.Lat = result.EndLat;
                scooter.Lang = result.EndLang;
                tblScooter.Update(scooter);

                return await Task.FromResult(Ok());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BadRequest(ex.Message));
            }
        }

        private decimal GetUserRemainder(string userId)
        {
            decimal entranceTotal = 0, exitTotal = 0;
            var result = tblRemainder.GetList(x => x.UserId == userId).GroupBy(x => x.RemainderType).Select(g => new { remainderType = g.Key, sum = g.Sum(x => x.Amount) });
            var entrance = result.FirstOrDefault(x => x.remainderType == RemainderType.Entrance);
            if (entrance is not null) entranceTotal = entrance.sum;
            var exit = result.FirstOrDefault(x => x.remainderType == RemainderType.Exit);
            if (exit is not null) exitTotal = exit.sum;
            return entranceTotal - exitTotal;
        }
    }
}
