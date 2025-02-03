using Authentication.Aplication.Config;
using Authentication.Infrastructure.Persistence;
using Authentication.Infrastructure.Repositories;

namespace Authentication.WebApi.Ioc;

/// <summary>
/// IoC configuration class for service services.
/// </summary>
public static class ServiceIoc
{
	/// <summary>
	/// Configures IoC container for services.
	/// </summary>
	/// <param name="services">The service collection.</param>
	public static void ConfigureServiceIoc(this IServiceCollection services)
	{
		services
			.ConfigureRepositoryIoc()
			.ConfigureValidator()
			.AddMediatR();
	}

	/// <summary>
	/// Configures IoC container for repositories.
	/// </summary>
	/// <param name="services"></param>
	private static IServiceCollection ConfigureRepositoryIoc(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IAuthRepository, AuthRepository>();
		return services;
	}
}
