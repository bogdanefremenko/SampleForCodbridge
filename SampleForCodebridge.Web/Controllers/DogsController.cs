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
		
		var totalItems = dogs.Count;
		//some errors
		dogs = dogs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

		var result = new
		{
			TotalItems = totalItems,
			PageNumber = pageNumber,
			PageSize = pageSize,
			Dogs = dogs
		};
		return Ok(result);
	}

	[HttpPost("/dog")]
	public async Task<IActionResult> Index([FromBody] Dog dog)
	{
		try
		{
			await _mediator.Send(new AddDogCommand(dog));
			return CreatedAtAction(nameof(GetDog), new { id = dog.Id }, dog);
		}
		catch
		{
			return StatusCode(500, "An error occurred while creating the dog.");
		}
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