using MediatR;
using SampleForBridgecode.Business.Validators;
using SampleForCodebridge.Core.Models;
using SampleForCodebridge.Data;

namespace SampleForBridgecode.Business.Cqrs.Commands;

public record AddDogCommand(Dog Dog) : IRequest;

internal class AddDogCommandHandler : IRequestHandler<AddDogCommand>
{
	private readonly DatabaseContext _context;

	public AddDogCommandHandler(DatabaseContext context)
	{
		_context = context;
	}

	public async Task Handle(AddDogCommand request, CancellationToken cancellationToken)
	{
		var validator = new DogValidator();
		var results = await validator.ValidateAsync(request.Dog, cancellationToken);

		if (results.IsValid)
		{
			_context.Set<Dog>().Add(request.Dog);
			await _context.SaveChangesAsync(cancellationToken);
			return;
		}

		//TODO: convert into codes
		foreach (var failure in results.Errors)
		{
			Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
		}
	}
}