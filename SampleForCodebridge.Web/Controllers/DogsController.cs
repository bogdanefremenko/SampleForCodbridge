using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleForBridgecode.Business.Cqrs.Commands;
using SampleForBridgecode.Business.Cqrs.Queries;
using SampleForCodebridge.Core.Models;

namespace SampleForCodebridge.Web.Controllers;

[ApiController]
public class DogsController : ControllerBase
{
	private readonly IMediator _mediator;

	public DogsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("/dogs")]
	public async Task<IActionResult> Index([FromQuery] string? attribute, [FromQuery] string? order, [FromQuery] int pageNumber, [FromQuery] int pageSize)
	{
		var result = await _mediator.Send(new GetAllDogsQuery(attribute, order, pageNumber, pageSize));
		return Ok(result);
	}
	
	[HttpPost("/dog")]
	public async Task<IActionResult> Index([FromBody] Dog dog)
	{
		await _mediator.Send(new AddDogCommand(dog));
		return CreatedAtAction(nameof(GetDog), new { id = dog.Id }, dog);
	}

	[HttpGet("/dog/{id:int}")]
	public async Task<ActionResult<Dog>> GetDog(int id)
	{
		var dog = await _mediator.Send(new SearchDogByIdQuery(id));
		return Ok(dog);
	}
}