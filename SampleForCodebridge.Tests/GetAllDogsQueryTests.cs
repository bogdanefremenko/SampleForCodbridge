using SampleForBridgecode.Business.Cqrs.Queries;
using SampleForCodebridge.Core.Models;

namespace SampleForCodebridge.Tests;

public class GetAllDogsQueryTests
{
	[Fact]
	public async Task GetAllDogsQueryHandler_ReturnsListOfDogs()
	{
		await using var context = await TestDatabaseCreator.CreateContext();
		
		context.Dogs.AddRange(
			new Dog { Name = "Sirko", Color = "Broun", TailLength = 8, Weight = 15 },
			new Dog { Name = "Reks", Color = "Grey", TailLength = 12, Weight = 25 },
			new Dog { Name = "Patron", Color = "White", TailLength = 6, Weight = 10 }
		);
		await context.SaveChangesAsync();
		
		var handler = new GetAllDogsQueryHandler(context);
		
		var query = new GetAllDogsQuery(null, null);
		
		var result = await handler.Handle(query, CancellationToken.None);
	
		
		
		
		
		
		// Assert.Equal(3, result.Count);
		// Assert.Contains(result, dog => dog.Name == "Sirko");
		// Assert.Contains(result, dog => dog.Name == "Reks");
		// Assert.Contains(result, dog => dog.Name == "Patron");
		
	}
}