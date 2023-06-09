﻿using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleForCodebridge.Core.Models;
using SampleForCodebridge.Data;

[assembly: InternalsVisibleTo("SampleForCodebridge.Tests")]
namespace SampleForBridgecode.Business.Cqrs.Queries;

public record SearchDogByIdQuery(int Id) : IRequest<Dog>;

internal class SearchDogByIdQueryHandler : IRequestHandler<SearchDogByIdQuery, Dog>
{
	private readonly DatabaseContext _context;

	public SearchDogByIdQueryHandler(DatabaseContext context)
	{
		_context = context;
	}

	public async Task<Dog> Handle(SearchDogByIdQuery request, CancellationToken cancellationToken)
	{
		var result = await _context.Dogs.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
		if (result is null)
			throw new ArgumentException("There isn't any dog with Id: " + request.Id);
		return result;
	}
}