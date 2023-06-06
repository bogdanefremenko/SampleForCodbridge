using Microsoft.EntityFrameworkCore;
using SampleForCodebridge.Core.Models;
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

			// context.AddRange(
			// 	new Dog { Name = "Sirko", Color = "Broun", TailLength = 8, Weight = 15 },
			// 	new Dog { Name = "Reks", Color = "Grey", TailLength = 12, Weight = 25 },
			// 	new Dog { Name = "Patron", Color = "White", TailLength = 6, Weight = 10 });
			//context.SaveChanges();
		}

		_databaseInitialized = true;
	}

	public DatabaseContext CreateContext()
		=> new DatabaseContext(
			new DbContextOptionsBuilder<DatabaseContext>()
				.UseSqlServer(ConnectionString)
				.Options);
}