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

					ResultDetails errorDetails;

					if (contextFeature.Error is VacancyParsingException) {

						// Если это ошибка парсинга
						VacancyParsingException exception = contextFeature.Error as VacancyParsingException;

						logger.LogError($"Ошибка парсинга вакансии с ид {exception.HeadHunterId}: {exception.Message}");
						errorDetails = VacancyParsingExceptionErrorDetails(context, exception);

					} else if (contextFeature.Error is ArchivedVacancyException) {

						// Вакансия в архиве
						context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
						errorDetails = ExceptionMessageErrorDetails(context, (ArchivedVacancyException)contextFeature.Error);						
					
					} else {

						// Если это какая-либо другая ошибка
						logger.LogError($"Something went wrong: {contextFeature.Error}");
						errorDetails = UnknownExceptionErrorDetails(context, contextFeature.Error);
					}

					errorDetails.StatusCode = context.Response.StatusCode;

					await context.Response.WriteAsync(errorDetails.ToString());
				});
			});
		}

		/// <summary>
		/// Создание ответа на основе сообщения в ошибке.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="error"></param>
		/// <returns></returns>
		private static ResultDetails ExceptionMessageErrorDetails(HttpContext context, ArchivedVacancyException error) =>
			new ResultDetails {

				// StatusCode = context.Response.StatusCode,
				Message = error.Message
			};

		/// <summary>
		/// Создание ответа о неизвестной ошибке.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="error"></param>
		/// <returns></returns>
		private static ResultDetails UnknownExceptionErrorDetails(HttpContext context, Exception error) =>
			new ResultDetails() {

				// StatusCode = context.Response.StatusCode,
				Message = "Internal Server Error."
			};

		/// <summary>
		/// Создание ответа об ошибке парсинга страницы вакансии.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="exception"></param>
		/// <returns></returns>
		private static ResultDetails VacancyParsingExceptionErrorDetails(HttpContext context, VacancyParsingException exception) =>
			new ResultDetails {

				// StatusCode = context.Response.StatusCode,
				Message = $"Произошла ошибка при парсинге вакансии с ид {exception.HeadHunterId}. " +
								$"Проверьте переданный ид на правильность или свяжитесь с разработчиком."
			};
	}
}
