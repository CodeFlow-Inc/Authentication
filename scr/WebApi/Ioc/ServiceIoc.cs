using Authentication.Aplication.AppService;
using Authentication.Domain.Interface;
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
		services.ConfigureAppServiceIoc();
		services.ConfigureRepositoryIoc();
	}

	/// <summary>
	/// Configures IoC container for app services.
	/// </summary>
	/// <param name="services"></param>
	private static IServiceCollection ConfigureAppServiceIoc(this IServiceCollection services)
	{
		services.AddScoped<IAuthAppService, AuthAppService>();
		return services;
	}

	/// <summary>
	/// Configures IoC container for repositories.
	/// </summary>
	/// <param name="services"></param>
	private static IServiceCollection ConfigureRepositoryIoc(this IServiceCollection services)
	{
		services.AddScoped<IAuthRepository, AuthRepository>();
		return services;
	}
}
