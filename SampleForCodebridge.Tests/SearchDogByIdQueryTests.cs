using SampleForBridgecode.Business.Cqrs.Queries;
using SampleForCodebridge.Core.Models;

namespace SampleForCodebridge.Tests;

public class SearchDogByIdQueryTests
{
	[Fact]
	public async Task SearchDogByIdQueryHandler_ExistingId_ReturnsDog()
	{
		await using var context = await TestDatabaseCreator.CreateContext();
		
		var handler = new SearchDogByIdQueryHandler(context);

		context.Dogs.AddRange(
			new Dog { Name = "Lili", Color = "Brown", TailLength = 10, Weight = 20 },
			new Dog { Name = "Leya", Color = "Black", TailLength = 15, Weight = 25 }
		);
		await context.SaveChangesAsync();

		var query = new SearchDogByIdQuery(2);

		var result = await handler.Handle(query, CancellationToken.None);

		Assert.NotNull(result);
		Assert.Equal("Leya", result.Name);
	}
}