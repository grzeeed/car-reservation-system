namespace CarReservation.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using MediatR;
using CarReservation.Application.Commands.Cars;
using CarReservation.Application.Queries.Cars;
using CarReservation.Application.DTOs;
using CarReservation.Domain.Enums;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CarsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCars()
    {
        var query = new GetAllCarsQuery();
        var result = await _mediator.Send(query);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCarById(Guid id)
    {
        var query = new GetCarByIdQuery(id);
        var result = await _mediator.Send(query);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }

    [HttpGet("{id}/analytics")]
    public async Task<IActionResult> GetCarAnalytics(
        Guid id,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var query = new GetCarAnalyticsQuery(id, startDate, endDate);
        var result = await _mediator.Send(query);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : BadRequest(result.Error);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableCars(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] CarType? type = null,
        [FromQuery] string? location = null)
    {
        var query = new GetAvailableCarsQuery(startDate, endDate, type, location);
        var result = await _mediator.Send(query);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCar([FromBody] CreateCarCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? CreatedAtAction(nameof(GetCarById), new { id = result.Value.Id }, result.Value)
            : BadRequest(result.Error);
    }

    [HttpPut("{id}/location")]
    public async Task<IActionResult> UpdateCarLocation(Guid id, [FromBody] UpdateCarLocationRequest request)
    {
        var command = new UpdateCarLocationCommand(id, request.City, request.Address, request.Latitude, request.Longitude);
        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? Ok(new { Message = "Car location updated successfully" })
            : BadRequest(result.Error);
    }

    [HttpPut("{id}/maintenance")]
    public async Task<IActionResult> SetCarMaintenance(Guid id)
    {
        var command = new SetCarMaintenanceCommand(id);
        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? Ok(new { Message = "Car set to maintenance successfully" })
            : BadRequest(result.Error);
    }

    [HttpPut("{id}/available")]
    public async Task<IActionResult> SetCarAvailable(Guid id)
    {
        var command = new SetCarAvailableCommand(id);
        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? Ok(new { Message = "Car set to available successfully" })
            : BadRequest(result.Error);
    }

    [HttpPut("{id}/out-of-service")]
    public async Task<IActionResult> SetCarOutOfService(Guid id)
    {
        var command = new SetCarOutOfServiceCommand(id);
        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? Ok(new { Message = "Car set to out of service successfully" })
            : BadRequest(result.Error);
    }
}

public record UpdateCarLocationRequest(
    string City,
    string Address,
    double Latitude,
    double Longitude
);
