using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotSoSimpleEcommerce.Order.Domain.Commands;
using NotSoSimpleEcommerce.Order.Domain.Mappings;
using NotSoSimpleEcommerce.Repositories.Contracts;
using NotSoSimpleEcommerce.Shared.InOut.Requests;
using NotSoSimpleEcommerce.Shared.Models;

namespace NotSoSimpleEcommerce.Order.Controllers;

[ApiController]
[Authorize]
[Route("api/request")]
public sealed class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IReadEntityRepository<OrderEntity> _readRepository;
    
    public OrderController
    (
        IMediator mediator,
        IReadEntityRepository<OrderEntity> readRepository
    )
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _readRepository = readRepository?? throw new ArgumentNullException(nameof(readRepository));
    }
    
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var orders = await _readRepository.GetAll()
            .Include(order => order.Product)
            .ToListAsync(cancellationToken);
        
        if (!orders.Any())
            return NoContent();

        return Ok(orders.MapToResponse());
    }

    
    [HttpGet("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetStockByProductIdAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var order = await _readRepository.GetAll()
            .Include(order => order.Product)
            .Where(order => order.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
            return NotFound();

        return Ok(order.MapToResponse());
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<IActionResult> AddAsync([FromBody] OrderRequest orderRequest, CancellationToken cancellationToken)
    {
        var createdOrder = await _mediator.Send(orderRequest.MapToRegisterOrderCommand(), cancellationToken);
        return Created(Request.GetDisplayUrl(), createdOrder);
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateAsync
    (
        [FromRoute]int id, 
        [FromBody] OrderRequest orderRequest, 
        CancellationToken cancellationToken
    )
    {
        var order = await _readRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
            return NotFound();
            
        var updatedProduct = await _mediator.Send(orderRequest.MapToUpdateCommand(id), cancellationToken);
        return Ok(updatedProduct);
    }
    
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var product = await _readRepository.GetByIdAsync(id, cancellationToken);
        if (product is null)
            return NotFound();
            
        await _mediator.Send(new DeleteOrderCommand(id), cancellationToken);
        return Ok();
    }
}
