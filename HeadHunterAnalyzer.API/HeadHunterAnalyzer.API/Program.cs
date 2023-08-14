using Contracts.Logger;
using HeadHunterAnalyzer.API.Extensions;
using HeadHunterAnalyzer.API.Filters;
using Microsoft.AspNetCore.Mvc;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config")); 

// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.CofigureIisIntegration();
builder.Services.ConfigureLoggerService();

builder.Services.ConfigureSqlContext(builder.Configuration);

builder.Services.Configure<ApiBehaviorOptions>(opt => opt.SuppressModelStateInvalidFilter = true);

builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.ConfigureSwagger();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(setupAction => {

	setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "HeadHunterAnalyzer v1");
});

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILoggerManager>());
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
