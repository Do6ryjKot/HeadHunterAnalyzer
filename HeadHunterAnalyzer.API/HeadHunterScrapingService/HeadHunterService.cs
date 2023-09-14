﻿using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Contracts.HeadHunter;
using Entities.Models;
using HeadHunterScrapingService.Exceptions;
using System.Net;

namespace HeadHunterScrapingService {

	public class HeadHunterService : IHeadHunterService {

		private readonly HeadHunterHttpClient _httpClient;

		private Exception? _error;

		private IHtmlDocument? _parsedPage;

		private IHtmlDocument ParsedPage {

			get { 
				
				if (_parsedPage == null) {

					throw new NullParsedDocumentException("No parsed page in object storage. Use LoadVacancyAsync first");
				}

				return _parsedPage;
			}

			set => _parsedPage = value;
		}


		public HeadHunterService() {

			_httpClient = new HeadHunterHttpClient();
		}

		/// <summary>
		/// Проверка наличия ошибок при загрузки страницы
		/// </summary>
		private void CheckErrors() {

			if (_error != null)
				throw _error;
		}

		/// <summary>
		/// Получение данных о компании из хранимой спаршенной страницы.
		/// </summary>
		/// <returns>Данные компании</returns>
		/// <exception cref="NullParsedDocumentException">Никакая страница до этого не была спаршена.</exception>
		/// <exception cref="NodeNotFoundException">Узел названия компании не найден.</exception>
		/// <exception cref="AttributeNotFoundException">Узел с названием комании не содержит атрибут href.</exception>
		/// <exception cref="IdNotFoundExeption">Ид не был найден в значении атрибута href.</exception>
		public Company GetCompany() {

			CheckErrors();

			// Получение ноды имени компании.
			var companyNameNode = ParsedPage.All.First(node => node.Attributes["data-qa"]?.Value == "vacancy-company-name");

			if (companyNameNode == null) {

				throw new NodeNotFoundException("Node with attribute vacancy-company-name not found in parsed page.");
			}

			string name = companyNameNode.TextContent;

			// Получение ид компании.
			string? href = companyNameNode.Attributes["href"]?.Value;

			if (string.IsNullOrEmpty(href)) {

				throw new AttributeNotFoundException("Attribute href not found in company name node.", "href");
			}

			string? idString = string.Concat(href.Where(symb => Char.IsDigit(symb)));

			int id;

			if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out id)) {

				throw new IdNotFoundExeption("Id not found in href of company name node");
			}

			return new Company { HeadHunterId = id, Name = name };			
		}

		/// <summary>
		/// Получение данных о вакансии из хранимой спаршенной страницы.
		/// </summary>
		/// <returns>Данные вакансии</returns>
		/// <exception cref="NodeNotFoundException">Узел названия вакансии не найден.</exception>
		public Vacancy GetVacancy() {

			CheckErrors();

			var vacancyNameNode = ParsedPage.All.First(node => node.Attributes["data-qa"]?.Value == "vacancy-title");

			if (vacancyNameNode == null) {

				throw new NodeNotFoundException("Node with attribute vacancy-title not found in parsed page.");
			}

			string name = vacancyNameNode.TextContent;

			return new Vacancy { Name = name };
		}

		/// <summary>
		/// Загрузка страницы, из которой необходимо далее получать данные.
		/// </summary>
		/// <param name="headHunterId">Ид вакансии, страница которой необходимо загружать.</param>
		/// <exception cref="NullParsedDocumentException">По какой-то причине документ не спарсился.</exception>
		/// <exception cref="VacancyNotFoundException">Вакансия с переданным ид не найдена.</exception>
		/// <exception cref="Exception">Непредвиденная ошибка.</exception>
		public async Task LoadVacancyAsync(int headHunterId) {

			// Получение данных страницы

			Stream pageData;
			_error = null;

			try {

				pageData = await _httpClient.GetVacancyData(headHunterId);

			} catch (HttpRequestException ex) {

				if (ex.StatusCode == HttpStatusCode.NotFound) {

					_error = new VacancyNotFoundException($"Vacancy with id {headHunterId} not exists");

					throw _error;
				}

				_error = new Exception("Unknown exception happened when trying get vacancy page", ex);

				throw _error;
			}


			// Парсинг страницы
			var document = await new HtmlParser().ParseDocumentAsync(pageData);

			if (document == null) {

				_error = new NullParsedDocumentException($"Parsed HTML document for HH id {headHunterId} got null value");

				throw _error;
			}

			// Сохранение спаршенной страницы
			ParsedPage = document;
		}
	}
}