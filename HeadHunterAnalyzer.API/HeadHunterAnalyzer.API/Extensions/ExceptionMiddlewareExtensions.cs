using Contracts.Logger;
using Entities.InformationModel;
using HeadHunterScrapingService.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace HeadHunterAnalyzer.API.Extensions {
	
	public static class ExceptionMiddlewareExtensions {

		public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger) {

			app.UseExceptionHandler(appError => {

				appError.Run(async context => {

					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					context.Response.ContentType = "application/json";

					var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

					if (contextFeature == null)
						return;

					// Если это ошибка парсинга
					if (contextFeature.Error is VacancyParsingException) {

						VacancyParsingException exeption = contextFeature.Error as VacancyParsingException;

						logger.LogError($"Ошибка парсинга вакансии с ид {exeption.HeadHunterId}: {exeption.Message}");
						await context.Response.WriteAsync(new ResultDetails {

							StatusCode = context.Response.StatusCode, //StatusCodes.Status500InternalServerError,
							Message = $"Произошла ошибка при парсинге вакансии с ид {exeption.HeadHunterId}. " +
								$"Проверьте переданный ид на правильность или свяжитесь с разработчиком."

						}.ToString());

						return;
					}

					// Если это какая-либо другая ошибка
					logger.LogError($"Something went wrong: {contextFeature.Error}");

					await context.Response.WriteAsync(new ResultDetails() {

						StatusCode = context.Response.StatusCode,
						Message = "Internal Server Error."

					}.ToString());
					
				});
			});
		}
	}
}
