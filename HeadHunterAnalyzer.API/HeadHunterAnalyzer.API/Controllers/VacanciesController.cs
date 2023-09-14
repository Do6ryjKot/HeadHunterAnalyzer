using AutoMapper;
using Contracts.DataServices;
using Contracts.HeadHunter;
using Contracts.Logger;
using Entities.DataTransferObjects;
using Entities.ErrorModel;
using Entities.Models;
using HeadHunterAnalyzer.API.Filters;
using HeadHunterScrapingService.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace HeadHunterAnalyzer.API.Controllers {

	[Route("api/vacancy")]
	[ApiController]
	public class VacanciesController : Controller {

		private readonly IMapper _mapper;
		private readonly ILoggerManager _logger;
		private readonly IRepositoryManager _repositoryManager;
		private readonly IHeadHunterService _hhService;

		public VacanciesController(IMapper mapper, ILoggerManager logger, IRepositoryManager repositoryManager, IHeadHunterService hhService) {
			_mapper = mapper;
			_logger = logger;
			_repositoryManager = repositoryManager;
			_hhService = hhService;
		}

		[HttpGet]
		public async Task<IActionResult> LoadTestData() {

			Company company = new Company {
				HeadHunterId = 111111111,
				Name = "Test Company"
			};

			_repositoryManager.Companies.CreateCompany(company);

			Vacancy vacancy = new Vacancy {

				HeadHunterId = 123123123,
				Name = "Test Vacancy",
				Company = company,
				Words = new List<Word> { 
					
					new Word { Value = ".NET" }
				}
			};

			_repositoryManager.Vacancies.CreateVacancy(vacancy);

			await _repositoryManager.SaveAsync();

			return Ok(vacancy);
		}

		/// <summary>
		/// Сохранение вакансии с набором слов.
		/// </summary>
		/// <param name="vacancyData">Данные вакансии.</param>
		/// <response code="200">Вакансия сохранена успешно.</response>
		/// <response code="400">Вакансия с ид id уже существует.</response>
		/// <response code="400">Если параметр null.</response>
		/// <response code="422">Если параметр не прошел валидацию.</response>
		/// <response code="500">Произошла ошибка при парсинге вакансии с ид id. Проверьте переданный ид на правильность или свяжитесь с разработчиком.</response>
		[HttpPost]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		public async Task<IActionResult> SaveAnalyzedVacancy([FromBody] VacancyForCreationDto vacancyData) {

			// Проверка на существование вакансии.
			var vacancy = await _repositoryManager.Vacancies.GetVacancyAsync(vacancyData.HeadHunterId, trackChanges: false);

			if (vacancy != null) {

				_logger.LogError($"Вакасия с ид {vacancyData.HeadHunterId} уже существует.");
				return BadRequest(new ErrorDetails() { StatusCode = StatusCodes.Status400BadRequest, Message = $"Вакасия с ид {vacancyData.HeadHunterId} уже существует." });
			}

			// Загрузка страницы, что будет парсится дальше.
			try {

				await _hhService.LoadVacancyAsync(vacancyData.HeadHunterId);

			} catch (VacancyParsingException parsingException) {

				_logger.LogError($"Ошибка парсинга вакансии с ид {vacancyData.HeadHunterId}: {parsingException.Message}");
				return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDetails {

					StatusCode = StatusCodes.Status500InternalServerError,
					Message = $"Произошла ошибка при парсинге вакансии с ид {vacancyData.HeadHunterId}. " +
						$"Проверьте переданный ид на правильность или свяжитесь с разработчиком."
				});
			}

			// Получение данных о предприятися со страницы вакансии.
			Company companyFromPage = _hhService.GetCompany();
			
			// Проверка существования компании в БД.
			Company? companyEntity = await _repositoryManager.Companies
				.GetCompanyByHhIdAsync(companyFromPage.HeadHunterId, trackChanges: false);

			if (companyEntity == null) {

				companyEntity = companyFromPage;
			}

			// Получение данных о вакансии со страницы вакансии.
			vacancy = _hhService.GetVacancy();

			List<Word> words = _mapper.Map<IEnumerable<Word>>(vacancyData.Words).ToList();

			vacancy.HeadHunterId = vacancyData.HeadHunterId;
			vacancy.Words = words;
			vacancy.Company = companyEntity;

			_repositoryManager.Vacancies.CreateVacancy(vacancy);

			await _repositoryManager.SaveAsync();

			return Ok("Вакансия сохранена успешно.");
		}
	}
}
