using System.Runtime.CompilerServices;
using MediatR;
using SampleForCodebridge.Core.Models;
using SampleForCodebridge.Data;

[assembly: InternalsVisibleTo("SampleForCodebridge.Tests")]

namespace SampleForBridgecode.Business.Cqrs.Queries;

public record GetAllDogsQuery(string? Attribute, string? Order, int PageNumber = 1, int PageSize = 10) : IRequest<List<Dog>>;

internal class GetAllDogsQueryHandler : IRequestHandler<GetAllDogsQuery, List<Dog>>
{
	private readonly DatabaseContext _context;

	public GetAllDogsQueryHandler(DatabaseContext context)
	{
		_context = context;
	}

	public async Task<List<Dog>> Handle(GetAllDogsQuery request, CancellationToken cancellationToken)
	{
		if (request.PageNumber <= 0 || request.PageSize <= 0)
			throw new AggregateException("Page number or size is invalid");
		
		var query = _context.Dogs.ToList();

		if (string.IsNullOrEmpty(request.Attribute))
			return query;

		var isDescending = string.Equals(request.Order, "desc", StringComparison.OrdinalIgnoreCase);
		Func<Dog, object> propertySelector = request.Attribute.ToLower() switch
		{
			"name" => d => d.Name,
			"color" => d => d.Color,
			"tail_length" => d => d.TailLength,
			"weight" => d => d.Weight,
			_ => throw new ArgumentException("Invalid attribute.")
		};

		var dogs = isDescending ? query.OrderByDescending(propertySelector).ToList() : query.OrderBy(propertySelector).ToList();

		var result = dogs.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

		return result;
	}
}