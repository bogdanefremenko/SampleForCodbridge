using Microsoft.EntityFrameworkCore;
using SampleForCodebridge.Core.Models;

namespace SampleForCodebridge.Data;

public interface IDatabaseContext
{
	DbSet<Dog> Dogs { get; }
	Task<int> SaveChangesAsync();
}