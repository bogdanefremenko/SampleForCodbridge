using Microsoft.EntityFrameworkCore;
using SampleForCodebridge.Core.Models;

namespace SampleForCodebridge.Data;

public sealed class DatabaseContext : DbContext
{
	public DbSet<Dog> Dogs { get; set; } = null!;

	public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
	{
		Database.EnsureCreated();
	}
}