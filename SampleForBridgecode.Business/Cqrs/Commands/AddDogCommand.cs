using System.Runtime.CompilerServices;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SampleForBridgecode.Business.Validators;
using SampleForCodebridge.Core.Models;
using SampleForCodebridge.Data;

[assembly: InternalsVisibleTo("SampleForCodebridge.Tests")]
namespace SampleForBridgecode.Business.Cqrs.Commands;

public record AddDogCommand(Dog Dog) : IRequest;

internal class AddDogCommandHandler : IRequestHandler<AddDogCommand>
{
	private readonly DatabaseContext _context;
	private readonly IValidator<Dog> _validator;

	public AddDogCommandHandler(DatabaseContext context, IValidator<Dog> validator)
	{
		_context = context;
		_validator = validator;
	}

	
	public async Task Handle(AddDogCommand request, CancellationToken cancellationToken)
	{
		var results = await _validator.ValidateAsync(request.Dog, cancellationToken);

		if (results.IsValid)
		{
			_context.Set<Dog>().Add(request.Dog);
			await _context.SaveChangesAsync(cancellationToken);
			return;
		}

		var exceptionString = results.Errors.Aggregate<ValidationFailure?, string>(default!, (current, failure) => current + ("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage + "\n"));

		throw new ArgumentException(exceptionString);
	}
}