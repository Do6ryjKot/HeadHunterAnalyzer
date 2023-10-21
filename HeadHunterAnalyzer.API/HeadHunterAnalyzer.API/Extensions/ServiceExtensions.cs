using Contracts.HeadHunter;
using Contracts.Logger;
using Entities;
using HeadHunterAnalyzer.API.Managers;
using HeadHunterScrapingService;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace HeadHunterAnalyzer.API.Extensions {
	
	/// <summary>
	/// Класс с методами расширения для контейнера сервисов.
	/// </summary>
	public static class ServiceExtensions {

		/// <summary>
		/// Настройка CORS.
		/// </summary>
		/// <param name="services"></param>
		public static void ConfigureCors(this IServiceCollection services) =>
			services.AddCors(opt => {

				opt.AddPolicy("CorsPolicy", builder =>
					builder.AllowAnyOrigin()
						.WithMethods("GET", "POST")
						.AllowAnyHeader());
			});

		/// <summary>
		/// Дефолтная настройка IIS.
		/// </summary>
		/// <param name="services"></param>
		public static void CofigureIisIntegration(this IServiceCollection services) =>
			services.Configure<IISOptions>(opt => { });

		/// <summary>
		/// Настройка подключения к БД.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
			services.AddDbContext<RepositoryContext>(opt =>
				opt.UseSqlServer(configuration.GetConnectionString("sqlConnection"), opt => 
					opt.MigrationsAssembly("HeadHunterAnalyzer.API")));

		/// <summary>
		/// Добавление логгера в пулл сервисов.
		/// </summary>
		/// <param name="services"></param>
		public static void ConfigureLoggerService(this IServiceCollection services) =>
			services.AddScoped<ILoggerManager, LoggerManager>();

		/// <summary>
		/// Настройка Сваггера.
		/// </summary>
		/// <param name="services"></param>
		public static void ConfigureSwagger(this IServiceCollection services) =>
			services.AddSwaggerGen(setupAction => {

				setupAction.SwaggerDoc("v1", new OpenApiInfo { Title = "HeadHunterAnalyzer API" });

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				setupAction.IncludeXmlComments(xmlPath);
			});

		/// <summary>
		/// Настройка используемых HTTP клиентов.
		/// </summary>
		/// <param name="services"></param>
		public static void ConfigureHttpClients(this IServiceCollection services) =>
			services.AddHttpClient<HeadHunterHttpClient>(client => { 
				
				client.BaseAddress = new Uri("https://hh.ru");
			});

		public static void ConfigureExternalServices(this IServiceCollection services) {

			services.AddScoped<IHeadHunterService, HeadHunterService>();
		}

		public static void ConfigureManagers(this IServiceCollection services) {

			services.AddScoped<IVacanciesManager, VacanciesManager>();
		}
	}
}
