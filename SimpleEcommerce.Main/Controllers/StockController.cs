using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Main.InOut.Requests;
using SimpleEcommerce.Main.Models;
using SimpleEcommerce.Main.Repositories.Contracts;
using SimpleEcommerce.Main.Mappings;

namespace SimpleEcommerce.Main.Controllers
{
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
        public async Task<IActionResult> GetAllAsync()
        {
            var stocks = await _stockReadRepository.GetAllAsync();
            var stocksAsArray = stocks as StockEntity[] ?? stocks.ToArray();
            if (!stocksAsArray.Any())
                return NoContent();

            return Ok(stocksAsArray.MapToResponse());
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddAsync([FromBody] StockRequest stock, CancellationToken cancellationToken)
        {
            var createdStock = await _mediator.Send(stock.MapToRegisterProductStockCommand(), cancellationToken);
            return Created(Request.GetDisplayUrl(), createdStock);
        }
    }
}
