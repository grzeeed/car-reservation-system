namespace CarReservation.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using MediatR;
using CarReservation.Application.Commands.Reservations;
using CarReservation.Application.Queries.Reservations;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservationById(Guid id)
    {
        var query = new GetReservationByIdQuery(id);
        var result = await _mediator.Send(query);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetReservationsByCustomer(Guid customerId)
    {
        var query = new GetReservationsByCustomerQuery(customerId);
        var result = await _mediator.Send(query);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> ReserveCar([FromBody] ReserveCarCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? CreatedAtAction(nameof(GetReservationById), new { id = result.Value.Id }, result.Value)
            : BadRequest(result.Error);
    }

    [HttpPut("{id}/confirm")]
    public async Task<IActionResult> ConfirmReservation(Guid id)
    {
        var command = new ConfirmReservationCommand(id);
        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? NoContent() 
            : BadRequest(result.Error);
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelReservation(Guid id, [FromBody] CancelReservationRequest request)
    {
        var command = new CancelReservationCommand(id, request.Reason);
        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? NoContent() 
            : BadRequest(result.Error);
    }
}

public record CancelReservationRequest(string Reason);
