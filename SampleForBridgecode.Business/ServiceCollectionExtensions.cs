using Microsoft.Extensions.DependencyInjection;

namespace SampleForBridgecode.Business;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddBusiness(this IServiceCollection services)
	{
		return services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
	}
}