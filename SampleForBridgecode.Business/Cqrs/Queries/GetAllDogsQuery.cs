using System.Runtime.CompilerServices;
using MediatR;
using SampleForCodebridge.Core.Models;
using SampleForCodebridge.Data;

[assembly: InternalsVisibleTo("SampleForCodebridge.Tests")]
namespace SampleForBridgecode.Business.Cqrs.Queries;

public record GetAllDogsQuery(string? Attribute, string? Order) : IRequest<List<Dog>>;

internal class GetAllDogsQueryHandler : IRequestHandler<GetAllDogsQuery, List<Dog>>
{
	private readonly DatabaseContext _context;

	public GetAllDogsQueryHandler(DatabaseContext context)
	{
		_context = context;
	}

	public async Task<List<Dog>> Handle(GetAllDogsQuery request, CancellationToken cancellationToken)
	{
		IQueryable<Dog> query = _context.Dogs;

		if (string.IsNullOrEmpty(request.Attribute)) return query.ToList();
		var isDescending = string.Equals(request.Order, "desc", StringComparison.OrdinalIgnoreCase);
		query = request.Attribute.ToLower() switch
		{
			"name" => isDescending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
			"color" => isDescending ? query.OrderByDescending(d => d.Color) : query.OrderBy(d => d.Color),
			"tail_length" => isDescending ? query.OrderByDescending(d => d.TailLength) : query.OrderBy(d => d.TailLength),
			"weight" => isDescending ? query.OrderByDescending(d => d.Weight) : query.OrderBy(d => d.Weight),
			_ => throw new ArgumentException("Invalid attribute.")
		};

		return query.ToList();
	}
}