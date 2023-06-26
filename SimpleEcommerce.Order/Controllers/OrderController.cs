using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Order.InOut.Requests;
using SimpleEcommerce.Order.Mappings;

namespace SimpleEcommerce.Order.Controllers;

[ApiController]
[Route("api/shopping")]
public sealed class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrderController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<IActionResult> AddAsync([FromBody] OrderRequest order, CancellationToken cancellationToken)
    {
        var createdOrder = await _mediator.Send(order.MapToRegisterOrderCommand(), cancellationToken);
        return Created(Request.GetDisplayUrl(), createdOrder);
    }
}
