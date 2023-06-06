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
	public async Task<IActionResult> Index([FromQuery] string? attribute, [FromQuery] string? order, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var dogs = await _mediator.Send(new GetAllDogsQuery(attribute, order));

		dogs = dogs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

		var result = new
		{
			TotalItems = dogs.Count,
			PageNumber = pageNumber,
			PageSize = pageSize,
			Dogs = dogs
		};
		return Ok(result);
	}

	[HttpPost("/dog")]
	public async Task<IActionResult> Index([FromBody] Dog dog)
	{
		await _mediator.Send(new AddDogCommand(dog));
		return CreatedAtAction(nameof(GetDog), new { id = dog.Id }, dog);
	}

	[HttpGet("/dogs/{id}")]
	public async Task<ActionResult<Dog>> GetDog(int id)
	{
		var dog = await _mediator.Send(new SearchDogByIdQuery(id));

		if (dog == null)
			return NotFound();

		return Ok(dog);
	}
}