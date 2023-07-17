using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotSoSimpleEcommerce.Main.Domain.Mappings;
using NotSoSimpleEcommerce.Main.Domain.Models;
using NotSoSimpleEcommerce.Main.Domain.Repositories.Contracts;

namespace NotSoSimpleEcommerce.Main.Controllers
{
    [Authorize]
    [Route("api/stock")]
    [ApiController]
    public sealed class StockController : ControllerBase
    {
        private readonly IStockReadRepository _stockReadRepository;
        public StockController(IStockReadRepository stockReadRepository)
        {
            _stockReadRepository = stockReadRepository ?? throw new ArgumentNullException(nameof(stockReadRepository));
        }
        
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var stocks = await _stockReadRepository.GetAllAsync(cancellationToken);
            var stockArray = stocks as StockEntity[] ?? stocks.ToArray();
            if (!stockArray.Any())
                return NoContent();

            return Ok(stockArray.MapToResponse());
        }
    }
}
