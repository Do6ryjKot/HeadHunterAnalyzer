using AutoMapper;
using Contracts.DataServices;
using Contracts.HeadHunter;
using Contracts.Logger;
using Entities.DataTransferObjects;
using Entities.ErrorModel;
using Entities.Models;
using HeadHunterAnalyzer.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HeadHunterAnalyzer.API.Controllers {

	[Route("api/vacancies")]
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

			await _hhService.LoadVacancyAsync(vacancyData.HeadHunterId);

			// Получение данных о предприятися со страницы вакансии.
			Company companyFromPage = _hhService.GetCompany();

			// Получение данных о вакансии со страницы вакансии.
			vacancy = _hhService.GetVacancy();



			// Проверка существования компании в БД.
			Company? companyEntity = await _repositoryManager.Companies
				.GetCompanyByHhIdAsync(companyFromPage.HeadHunterId, trackChanges: true);

			if (companyEntity == null) {

				vacancy.Company = companyFromPage;

			} else {

				companyEntity.Vacancies.Add(vacancy);
			}



			List<Word> words = _mapper.Map<IEnumerable<Word>>(vacancyData.Words).ToList();

			IEnumerable<string> wordValues = words.Select(word => word.Value);
			IEnumerable<Word> existingWords = await _repositoryManager.Words.GetWordsByValuesAsync(wordValues, trackChanges: true);

			IEnumerable<Word> newWords = words.Where(word =>
				!existingWords.Select(exWord => exWord.Value).Contains(word.Value));

			foreach (var word in newWords) {

				// _repositoryManager.Words.CreateWord(word);
				vacancy.Words.Add(word);
			}

			foreach (var word in existingWords)
				word.Vacancies.Add(vacancy);




			_repositoryManager.Vacancies.CreateVacancy(vacancy);

			await _repositoryManager.SaveAsync();

			return Ok("Вакансия сохранена успешно.");
		}

		/// <summary>
		/// Возвращает данные по вакансии.
		/// </summary>
		/// <param name="headHunterId">Ид вакансии на ХХ.</param>
		/// <returns>Данные вакансии</returns>
		[HttpGet("{headHunterId}")]
		public async Task<IActionResult> AnalyzeVacancy(int headHunterId) {

			await _hhService.LoadVacancyAsync(headHunterId);


			AnalyzedVacancyDto result = new AnalyzedVacancyDto();


			VacancyData vacancyData = _hhService.GetVacancyData();
			result.VacancyData = vacancyData;


			Company company = _hhService.GetCompany();
			result.Company = _mapper.Map<AnalyzedCompanyDto>(company);


			IEnumerable<Word> words;

			Vacancy? vacancyEntity = await _repositoryManager.Vacancies.GetVacancyAsync(headHunterId, trackChanges: false);

			if (vacancyEntity == null) {

				result.AlreadyAnalyzed = false;

				List<string> vacancyWords = _hhService.GetVacancyWords().ToList();
				words = await _repositoryManager.Words.GetWordsByValuesAsync(vacancyWords, trackChanges: false);

			} else { 
			
				result.AlreadyAnalyzed = true;

				words = await _repositoryManager.Words.GetWordsByVacancyIdAsync(vacancyEntity.Id, trackChanges: false);
			}

			
			result.Words = _mapper.Map<IEnumerable<WordDto>>(words);

			return Ok(result);
		}
	}
}
