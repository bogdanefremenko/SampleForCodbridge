using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleForCodebridge.Core.Models;
using SampleForCodebridge.Data;

[assembly: InternalsVisibleTo("SampleForCodebridge.Tests")]
namespace SampleForBridgecode.Business.Cqrs.Queries;

public record SearchDogByIdQuery(int Id) : IRequest<Dog?>;

internal class SearchDogByIdQueryHandler : IRequestHandler<SearchDogByIdQuery, Dog?>
{
	private readonly DatabaseContext _context;

	public SearchDogByIdQueryHandler(DatabaseContext context)
	{
		_context = context;
	}

	public async Task<Dog?> Handle(SearchDogByIdQuery request, CancellationToken cancellationToken)
	{
		return await _context.Dogs.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
	}
}