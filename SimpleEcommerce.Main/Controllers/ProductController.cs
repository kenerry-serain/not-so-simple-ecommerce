using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Main.Commands;
using SimpleEcommerce.Main.InOut.Requests;
using SimpleEcommerce.Main.Models;
using SimpleEcommerce.Main.Repositories.Contracts;
using SimpleEcommerce.Main.Mappings;

namespace SimpleEcommerce.Main.Controllers
{
    [Route("api/product")]
    [ApiController]
    public sealed class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IProductReadRepository _productReadRepository;

        public ProductController(IMediator mediator, IProductReadRepository productReadRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _productReadRepository = productReadRepository ?? throw new ArgumentNullException(nameof(productReadRepository));
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _productReadRepository.GetAllAsync();
            var productsAsArrays = products as ProductEntity[] ?? products.ToArray();
            if (!productsAsArrays.Any())
                return NoContent();

            return Ok(productsAsArrays.MapToResponse());
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product is null)
                return NotFound();

            return Ok(product.MapToResponse());
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AddAsync([FromBody] ProductRequest productRequest, CancellationToken cancellationToken)
        {
            var createdProduct = await _mediator.Send(productRequest.MapToRegisterCommand(), cancellationToken);
            return Created(Request.GetDisplayUrl(),createdProduct);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync([FromRoute]int id, [FromBody] ProductRequest productRequest, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product is null)
                return NotFound();
            
            var updatedProduct = await _mediator.Send(productRequest.MapToUpdateCommand(id), cancellationToken);
            return Ok(updatedProduct);
        }
        
        [HttpPut("{id:int}/stock")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] StockRequest stock, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(stock.ProductId);
            if (product is null)
                return NotFound();
            
            var updatedStock = await _mediator.Send(stock.MapToUpdateProductStockCommand(), cancellationToken);
            return Ok(updatedStock);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product is null)
                return NotFound();
            
            await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return Ok();
        }
    }
}
