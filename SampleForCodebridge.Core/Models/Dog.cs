using System.ComponentModel.DataAnnotations;

namespace SampleForCodebridge.Core.Models;

public class Dog
{
	[Key] 
	public int Id { get; set; }
	
	[Required] 
	[MaxLength(50)] 
	public string Name { get; set; } = null!;

	[Required] 
	[MaxLength(50)] 
	public string Color { get; set; } = null!;

	[Required]
	public int TailLength { get; set; }

	[Required] 
	public int Weight { get; set; }
}