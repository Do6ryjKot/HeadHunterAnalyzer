using AutoMapper;
using Contracts.DataServices;
using Contracts.Logger;
using Entities.DataTransferObjects;
using Entities.InformationModel;
using Entities.RequestFeatures;
using HeadHunterAnalyzer.API.Filters;
using HeadHunterAnalyzer.API.Managers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HeadHunterAnalyzer.API.Controllers {

	[Route("api/vacancies")]
	[ApiController]
	public class VacanciesController : Controller {

		private readonly IMapper _mapper;
		private readonly ILoggerManager _logger;
		private readonly IRepositoryManager _repositoryManager;
		private readonly IVacanciesManager _vacanciesManager;

		public VacanciesController(IMapper mapper, ILoggerManager logger, IRepositoryManager repositoryManager, IVacanciesManager vacanciesManager) {

			_mapper = mapper;
			_logger = logger;
			_repositoryManager = repositoryManager;
			_vacanciesManager = vacanciesManager;
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
		/// <response code="500">Неизвестная ошибка.</response>
		[HttpPost]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		[ProducesResponseType(typeof(ResultDetails), 200)]
		[ProducesResponseType(typeof(ResultDetails), 400)]
		[ProducesResponseType(typeof(ResultDetails), 500)]
		public async Task<IActionResult> SaveAnalyzedVacancy([FromBody] VacancyForCreationDto vacancyData) {

			// Проверка на существование вакансии.
			var vacancy = await _repositoryManager.Vacancies.GetVacancyAsync(vacancyData.HeadHunterId, trackChanges: false);

			if (vacancy != null) {

				_logger.LogError($"Вакасия с ид {vacancyData.HeadHunterId} уже существует.");
				return BadRequest(new ResultDetails { StatusCode = StatusCodes.Status400BadRequest, Message = $"Вакасия с ид {vacancyData.HeadHunterId} уже существует." });
			}

			await _vacanciesManager.SaveVacancyAsync(vacancyData);

			return Ok(new ResultDetails { StatusCode = StatusCodes.Status200OK, Message = "Вакансия сохранена успешно." });
		}

		/// <summary>
		/// Возвращает данные по вакансии.
		/// </summary>
		/// <param name="headHunterId">Ид вакансии на ХХ.</param>
		/// <response code="200">Данные вакансии.</response>
		/// <response code="400">Вакансия находится в архиве.</response>
		/// <response code="500">Произошла ошибка при парсинге вакансии с ид id. Проверьте переданный ид на правильность или свяжитесь с разработчиком.</response>
		/// <response code="500">Неизвестная ошибка.</response>
		/// <returns>Данные вакансии.</returns>
		[HttpGet("{headHunterId}")]
		[ProducesResponseType(typeof(AnalyzedVacancyDto), 200)]
		[ProducesResponseType(typeof(ResultDetails), 400)]
		[ProducesResponseType(typeof(ResultDetails), 500)]
		public async Task<IActionResult> AnalyzeVacancy(int headHunterId) {

			var result = await _vacanciesManager.AnalyzeVacancyAsync(headHunterId);

			return Ok(result);
		}

		/// <summary>
		/// Возвращает набор вакансий, что были анализированы ранее.
		/// </summary>
		/// <param name="parameters">Параметры пагинации.</param>
		/// <response code="200">Набор вакансий.</response>
		/// <response code="500">Неизвестная ошибка.</response>
		/// <returns>Набор вакансий.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<VacancyDto>), 200)]
		[ProducesResponseType(typeof(ResultDetails), 500)]
		public async Task<IActionResult> GetAllAnalyzedVacancies([FromQuery] RequestParameters parameters) {

			var vacancies = await _repositoryManager.Vacancies.GetAllVacancies(parameters, trackChanges: false);

			Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(vacancies.Metadata));

			var vacanciesDto = _mapper.Map<IEnumerable<VacancyDto>>(vacancies);

			return Ok(vacanciesDto);
		}

		/// <summary>
		/// Добавляет слова в уже сохраненную вакансию. Слова, что уже связаны с этой вакансией будут игнорироваться.
		/// </summary>
		/// <param name="vacancyId">Внутренний ид сохраненной вакансии.</param>
		/// <param name="wordsToAdd">Набор слов, что необходимо добавить к вакансии.</param>
		/// <returns>Новый набор слов вакансии.</returns>
		[HttpPost("{vacancyId}/add-words")]
		[ProducesResponseType(typeof(IEnumerable<WordDto>), 200)]
		[ProducesResponseType(typeof(ResultDetails), 400)]
		[ProducesResponseType(typeof(ResultDetails), 500)]
		public async Task<IActionResult> AddWordsToAnalyzedVacancy(Guid vacancyId,
				[FromBody] IEnumerable<WordForCreationDto> wordsToAdd) {

			// Проверка на существование вакансии.
			var vacancy = await _repositoryManager.Vacancies.GetVacancyAsync(vacancyId, trackChanges: true);

			if (vacancy == null) {

				_logger.LogError($"Вакасия с ид {vacancyId} не существует.");
				return BadRequest(new ResultDetails {
					StatusCode = StatusCodes.Status400BadRequest,
					Message = $"Вакасия с ид {vacancyId} не существует."
				});
			}

			var newWords = await _vacanciesManager.AddWordsToVacancyAsync(vacancy, wordsToAdd);

			return Ok(newWords);
		}
	}
}
