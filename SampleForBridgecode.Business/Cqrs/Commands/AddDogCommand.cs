using System.Runtime.CompilerServices;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
		if (await _context.Dogs.AnyAsync(d => d.Name == request.Dog.Name, cancellationToken: cancellationToken))
			throw new ArgumentException("There is a dog with this name");
		if (!results.IsValid)
			throw new ValidationException(results.Errors);
		_context.Dogs.Add(request.Dog);
		await _context.SaveChangesAsync(cancellationToken);
	}
}