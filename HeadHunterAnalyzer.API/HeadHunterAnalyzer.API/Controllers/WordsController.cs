using AutoMapper;
using Contracts.DataServices;
using Contracts.Logger;
using Entities.DataTransferObjects;
using Entities.Models;
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
		/// Получить все добавленные слова
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetAllWords() {

			IEnumerable<Word> words = await _repositoryManager.Words.GetAllWords(trackChanges: false);

			IEnumerable<WordDto> result = _mapper.Map<IEnumerable<WordDto>>(words);

			return Ok(result);
		}
	}
}
