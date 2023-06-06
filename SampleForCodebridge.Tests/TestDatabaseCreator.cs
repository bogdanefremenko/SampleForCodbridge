using Microsoft.EntityFrameworkCore;
using SampleForCodebridge.Data;

namespace SampleForCodebridge.Tests;

public static class TestDatabaseCreator
{
	private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=DogsTest;Trusted_Connection=True";

	public static async Task<DatabaseContext> CreateContext()
	{
		var databaseContext = new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>().UseSqlServer(ConnectionString).Options);
		await databaseContext.Database.EnsureDeletedAsync();
		await databaseContext.Database.EnsureCreatedAsync();
		return databaseContext;
	}
}