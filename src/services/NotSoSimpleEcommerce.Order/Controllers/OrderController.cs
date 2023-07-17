using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NotSoSimpleEcommerce.Main.Domain.Mappings;
using NotSoSimpleEcommerce.Order.Domain.Mappings;
using NotSoSimpleEcommerce.Order.Domain.Models;
using NotSoSimpleEcommerce.Repositories.Contracts;
using NotSoSimpleEcommerce.Shared.InOut.Requests;

namespace NotSoSimpleEcommerce.Order.Controllers;

[ApiController]
[Route("api/shopping")]
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
        var orders = await _readRepository.GetAllAsync(cancellationToken);
        var orderArray = orders as OrderEntity[] ?? orders.ToArray();
        if (!orderArray.Any())
            return NoContent();

        return Ok(orderArray.MapToResponse());
    }

    
    [HttpGet("order/{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetStockByProductIdAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var order = await _readRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
            return NotFound();

        return Ok(order.MapToResponse());
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<IActionResult> AddAsync([FromBody] OrderRequest order, CancellationToken cancellationToken)
    {
        var createdOrder = await _mediator.Send(order.MapToRegisterOrderCommand(), cancellationToken);
        return Created(Request.GetDisplayUrl(), createdOrder);
    }
}
