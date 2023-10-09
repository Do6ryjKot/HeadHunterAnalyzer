using Contracts.DataServices;
using Contracts.HeadHunter;
using Contracts.Logger;
using HeadHunterAnalyzer.API.Extensions;
using HeadHunterAnalyzer.API.Filters;
using HeadHunterScrapingService;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Repository;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config")); 

// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.CofigureIisIntegration();
builder.Services.ConfigureLoggerService();

builder.Services.ConfigureHttpClients();

builder.Services.ConfigureSqlContext(builder.Configuration);

builder.Services.Configure<ApiBehaviorOptions>(opt => opt.SuppressModelStateInvalidFilter = true);

builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.ConfigureSwagger();

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IHeadHunterService, HeadHunterService>();

builder.Services.AddControllers().AddNewtonsoftJson(opts =>
	opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(setupAction => {

	setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "HeadHunterAnalyzer v1");
});

app.ConfigureExceptionHandler(app.Services.CreateScope().ServiceProvider.GetRequiredService<ILoggerManager>());
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
