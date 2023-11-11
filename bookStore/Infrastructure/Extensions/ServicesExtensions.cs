using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Repositories.Contracts;
using Repositories.EfCore;
using Services.Concrete;
using Services.Contracts;

namespace bookStore.Infrastructure.Extensions
{
	public static class ServicesExtensions
	{
		public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<RepositoryContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
		}
		public static void ConfigureRepositoryRegistration(this IServiceCollection services)
		{
			services.AddScoped<IRepositoryManager, RepositoryManager>();
			services.AddScoped<IBookRepository, BookRepository>();
		}
		public static void ConfigureServiceRegistration(this IServiceCollection services)
		{
			services.AddScoped<IServiceManager, ServiceManager>();
			services.AddScoped<IBookService, BookManager>();

			services.AddSingleton<ILoggerService, LoggerManager>();
		}
		public static void ConfigureActionFilters(this IServiceCollection services)
		{
			services.AddScoped<ValidationFilterAttribute>();
			services.AddSingleton<LogFilterAttribute>();
		}
		public static void ConfigureCors(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy", builder =>
				builder.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.WithExposedHeaders("X-Pagination")
				);
			});
		}
	}
}
