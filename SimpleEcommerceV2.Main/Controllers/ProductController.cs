using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerceV2.Main.Domain.Commands;
using SimpleEcommerceV2.Main.Domain.InOut.Requests;
using SimpleEcommerceV2.Main.Domain.Mappings;
using SimpleEcommerceV2.Main.Domain.Models;
using SimpleEcommerceV2.Repositories.Contracts;

namespace SimpleEcommerceV2.Main.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/product")]
    public sealed class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IReadEntityRepository<ProductEntity> _readRepository;

        public ProductController(IMediator mediator, IReadEntityRepository<ProductEntity> readRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var products = await _readRepository.GetAllAsync(cancellationToken);
            var productsAsArrays = products as ProductEntity[] ?? products.ToArray();
            if (!productsAsArrays.Any())
                return NoContent();

            return Ok(productsAsArrays.MapToResponse());
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var product = await _readRepository.GetByIdAsync(id, cancellationToken);
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
            var product = await _readRepository.GetByIdAsync(id, cancellationToken);
            if (product is null)
                return NotFound();
            
            var updatedProduct = await _mediator.Send(productRequest.MapToUpdateCommand(id), cancellationToken);
            return Ok(updatedProduct);
        }
        
        [HttpPut("{id:int}/stock")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync([FromRoute]int id, [FromBody] StockRequest stock, CancellationToken cancellationToken)
        {
            var product = await _readRepository.GetByIdAsync(id, cancellationToken);
            if (product is null)
                return NotFound();
            
            var updatedStock = await _mediator.Send(stock.MapToUpdateProductStockCommand(id), cancellationToken);
            return Ok(updatedStock);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var product = await _readRepository.GetByIdAsync(id, cancellationToken);
            if (product is null)
                return NotFound();
            
            await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return Ok();
        }
    }
}
