using MediatR;
using SampleForBridgecode.Business.Cqrs.Commands;
using SampleForBridgecode.Business.Cqrs.Queries;
using SampleForCodebridge.Core.Models;

namespace SampleForCodebridge.Tests;

[Collection("Sequential")]
public class CqrsTests : IClassFixture<TestDatabaseFixture>
{
	private readonly TestDatabaseFixture  _fixture;

	public CqrsTests(TestDatabaseFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public async Task AddDogCommandHandler_ValidDog_DogAddedToContext()
	{
		await using var context = _fixture.CreateContext();
		
		var handler = new AddDogCommandHandler(context);
		var dog = new Dog { Name = "Barbos", Color = "Black", TailLength = 10, Weight = 20 };
		var command = new AddDogCommand(dog);
		
		await handler.Handle(command, CancellationToken.None);
		
		Assert.Equal(1, context.Dogs.Count());
		Assert.Equal(dog.Name, context.Dogs.Single().Name);
		await context.Database.EnsureDeletedAsync();
	}

	[Fact]
	public async Task GetAllDogsQueryHandler_ReturnsListOfDogs()
	{
		await using var context = _fixture.CreateContext();
		
		context.Dogs.AddRange(
			new Dog { Name = "Sirko", Color = "Broun", TailLength = 8, Weight = 15 },
			new Dog { Name = "Reks", Color = "Grey", TailLength = 12, Weight = 25 },
			new Dog { Name = "Patron", Color = "White", TailLength = 6, Weight = 10 }
		);
		await context.SaveChangesAsync();
		
		var handler = new GetAllDogsQueryHandler(context);
		
		var query = new GetAllDogsQuery(null, null);
		
		var result = await handler.Handle(query, CancellationToken.None);

		Assert.Equal(3, result.Count);
		Assert.Contains(result, dog => dog.Name == "Sirko");
		Assert.Contains(result, dog => dog.Name == "Reks");
		Assert.Contains(result, dog => dog.Name == "Patron");
		await context.Database.EnsureDeletedAsync();
	}

	[Fact]
	public async Task SearchDogByIdQueryHandler_ExistingId_ReturnsDog()
	{
		await using var context = _fixture.CreateContext();
		
		var handler = new SearchDogByIdQueryHandler(context);

		context.Dogs.AddRange(
			new Dog { Name = "Dog 1", Color = "Brown", TailLength = 10, Weight = 20 },
			new Dog { Name = "Dog 2", Color = "Black", TailLength = 15, Weight = 25 }
		);
		await context.SaveChangesAsync();

		var query = new SearchDogByIdQuery(2);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("Dog 2", result.Name);
		await context.Database.EnsureDeletedAsync();
	}
}