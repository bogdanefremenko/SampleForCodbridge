using Microsoft.Extensions.DependencyInjection;
using SampleForBridgecode.Business.Validators;
using FluentValidation;

namespace SampleForBridgecode.Business;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddBusiness(this IServiceCollection services)
	{
		services.AddValidatorsFromAssemblyContaining<DogValidator>();
		services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
		return services;
	}
}