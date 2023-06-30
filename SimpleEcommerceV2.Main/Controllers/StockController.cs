using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerceV2.Main.Domain.InOut.Requests;
using SimpleEcommerceV2.Main.Domain.Mappings;
using SimpleEcommerceV2.Main.Domain.Models;
using SimpleEcommerceV2.Main.Domain.Repositories.Contracts;

namespace SimpleEcommerceV2.Main.Controllers
{
    [Authorize]
    [Route("api/stock")]
    [ApiController]
    public sealed class StockController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStockReadRepository _stockReadRepository;

        public StockController(IMediator mediator, IStockReadRepository stockReadRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _stockReadRepository = stockReadRepository ?? throw new ArgumentNullException(nameof(stockReadRepository));
        }
        
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var stocks = await _stockReadRepository.GetAllAsync(cancellationToken);
            var stocksAsArray = stocks as StockEntity[] ?? stocks.ToArray();
            if (!stocksAsArray.Any())
                return NoContent();

            return Ok(stocksAsArray.MapToResponse());
        }
    }
}
