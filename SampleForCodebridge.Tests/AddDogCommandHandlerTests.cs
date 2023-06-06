using SampleForBridgecode.Business.Cqrs.Commands;
using SampleForBridgecode.Business.Validators;

namespace SampleForCodebridge.Tests;

public class AddDogCommandHandlerTests
{
	[Fact]
	public async Task AddDogCommandHandler_ValidDog_DogAddedToContext()
	{
		await using var context =  await TestDatabaseCreator.CreateContext();
		var validator = new DogValidator();
		
		var handler = new AddDogCommandHandler(context, validator);
		var dog = new Dog { Name = "Barbos", Color = "Black", TailLength = 10, Weight = 20 };
		var command = new AddDogCommand(dog);
		
		await handler.Handle(command, CancellationToken.None);
		
		Assert.Equal(1, context.Dogs.Count());
		Assert.Equal(dog.Name, context.Dogs.Single().Name);
	}
}