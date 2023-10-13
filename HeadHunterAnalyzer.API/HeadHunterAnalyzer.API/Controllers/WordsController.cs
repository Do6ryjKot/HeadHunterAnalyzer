using AutoMapper;
using Contracts.DataServices;
using Contracts.Logger;
using Entities.DataTransferObjects;
using Entities.InformationModel;
using Microsoft.AspNetCore.Mvc;

namespace HeadHunterAnalyzer.API.Controllers {

	[Route("api/words")]
	[ApiController]
	public class WordsController : Controller {

		private readonly IMapper _mapper;
		private readonly ILoggerManager _logger;
		private readonly IRepositoryManager _repositoryManager;

		public WordsController(IMapper mapper, ILoggerManager logger, IRepositoryManager repositoryManager) {
			_mapper = mapper;
			_logger = logger;
			_repositoryManager = repositoryManager;
		}

		/// <summary>
		/// Получить все добавленные слова.
		/// </summary>
		/// <returns>Набор слов.</returns>
		/// <response code="200">Набор слов с количеством вхождений в вакансии.</response>
		/// <response code="500">Неизвестная ошибка.</response>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<WordOccurrencesDto>), 200)]
		[ProducesResponseType(typeof(ResultDetails), 500)]
		public async Task<IActionResult> GetAllWords() {

			IEnumerable<WordOccurrencesDto> result = await _repositoryManager.Words.GetAllWordsOccurrences();

			return Ok(result);
		}
	}
}
