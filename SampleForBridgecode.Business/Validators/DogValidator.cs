using FluentValidation;
using SampleForCodebridge.Core.Models;

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