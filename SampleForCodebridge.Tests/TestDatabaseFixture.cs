using Microsoft.EntityFrameworkCore;
using SampleForCodebridge.Data;

namespace SampleForCodebridge.Tests;

public class TestDatabaseFixture
{
	private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=DogsTest;Trusted_Connection=True";

	private static readonly object Lock = new();
	private static bool _databaseInitialized;

	public TestDatabaseFixture()
	{
		if (_databaseInitialized) return;
		using (var context = CreateContext())
		{
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
		}

		_databaseInitialized = true;
	}

	public DatabaseContext CreateContext()
		=> new DatabaseContext(
			new DbContextOptionsBuilder<DatabaseContext>()
				.UseSqlServer(ConnectionString)
				.Options);
}