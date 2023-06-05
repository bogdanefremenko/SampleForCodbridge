using FluentValidation;
using SampleForBridgecode.Business.Cqrs.Commands;
using SampleForCodebridge.Core.Models;
using SampleForCodebridge.Data;

namespace SampleForBridgecode.Business.Validators;

public class DogValidator : AbstractValidator<Dog>
{
	public DogValidator()
	{
		RuleFor(dog => dog.Name ).NotNull().NotEmpty().Length(0, 50);
		RuleFor(dog => dog.Color ).NotNull().NotEmpty().Length(0, 50);
		RuleFor(dog => dog.TailLength );
		RuleFor(dog => dog.Weight );
	}
}