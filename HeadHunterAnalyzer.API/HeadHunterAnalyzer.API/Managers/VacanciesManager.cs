using AutoMapper;
using Contracts.DataServices;
using Contracts.HeadHunter;
using Contracts.Logger;
using Entities.DataTransferObjects;
using Entities.Models;

namespace HeadHunterAnalyzer.API.Managers {

	public class VacanciesManager : IVacanciesManager {

		private readonly IHeadHunterService _hhService;
		private readonly IRepositoryManager _repositoryManager;
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;

		public VacanciesManager(IHeadHunterService hhService, IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper) {
			_hhService = hhService;
			_repositoryManager = repositoryManager;
			_logger = logger;
			_mapper = mapper;
		}

		public async Task<AnalyzedVacancyDto> AnalyzeVacancyAsync(int headHunterId) {

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

			return result;
		}

		public async Task<IEnumerable<WordDto>> AddWordsToVacancyAsync(Vacancy vacancy, IEnumerable<WordForCreationDto> wordsDtos) {

			var words = _mapper.Map<IEnumerable<Word>>(wordsDtos);
			var wordsDtosValues = wordsDtos.Select(word => word.Value);

			var linkedWords = await _repositoryManager.Words.GetWordsByVacancyIdAndValues(vacancy.Id, wordsDtosValues, trackChanges: false);
			var existingWords = await _repositoryManager.Words.GetWordsByValuesAsync(wordsDtosValues, trackChanges: true);

			var wordsToLink = existingWords.Except(linkedWords, new WordComparer());
			var wordsToAdd = words.Except(existingWords, new WordComparer());

			foreach (Word word in wordsToAdd) {

				vacancy.Words.Add(word);
			}

			foreach (Word word in wordsToLink) { 
				
				word.Vacancies.Add(vacancy);
			}

			await _repositoryManager.SaveAsync();

			var newVacancyWords = await _repositoryManager.Words.GetWordsByVacancyIdAsync(vacancy.Id, trackChanges: false);

			return _mapper.Map<IEnumerable<WordDto>>(newVacancyWords);
		}

		public async Task<VacancyDto> SaveVacancyAsync(VacancyForCreationDto vacancyDto) {

			await _hhService.LoadVacancyAsync(vacancyDto.HeadHunterId);

			// Получение данных о предприятися со страницы вакансии.
			Company companyFromPage = _hhService.GetCompany();

			// Получение данных о вакансии со страницы вакансии.
			var vacancy = _hhService.GetVacancy();

			await SaveCompanyToVacancyAsync(vacancy, companyFromPage);

			List<Word> words = _mapper.Map<IEnumerable<Word>>(vacancyDto.Words).ToList();

			await SaveWordsToVacancyAsync(vacancy, words);

			_repositoryManager.Vacancies.CreateVacancy(vacancy);

			await _repositoryManager.SaveAsync();

			return _mapper.Map<VacancyDto>(vacancy);
		}

		/// <summary>
		/// Сохраняет слова к вакансии слова независимо от того, были ли они ранее добавлены. Используется при создании вакансии.
		/// </summary>
		/// <param name="vacancy">Вакансия.</param>
		/// <param name="words">Набор слов.</param>
		private async Task SaveWordsToVacancyAsync(Vacancy vacancy, IEnumerable<Word> words) {

			List<string> wordValues = words.Select(word => word.Value).ToList();
			IEnumerable<Word> existingWords = await _repositoryManager.Words.GetWordsByValuesAsync(wordValues, trackChanges: true);

			IEnumerable<Word> newWords = words.Where(word =>
				!existingWords.Select(exWord => exWord.Value).Contains(word.Value));

			foreach (var word in newWords) {

				vacancy.Words.Add(word);
			}

			foreach (var word in existingWords)
				word.Vacancies.Add(vacancy);
		}

		/// <summary>
		/// Сохранение компании к вакансии при создании новой вакансии.
		/// </summary>
		/// <param name="vacancy">Вакансия.</param>
		/// <param name="companyFromPage">Данные компании.</param>
		/// <returns></returns>
		private async Task SaveCompanyToVacancyAsync(Vacancy vacancy, Company companyFromPage) {

			// Проверка существования компании в БД.
			Company? companyEntity = await _repositoryManager.Companies
				.GetCompanyByHhIdAsync(companyFromPage.HeadHunterId, trackChanges: true);

			if (companyEntity == null) {

				vacancy.Company = companyFromPage;

			} else {
				companyEntity.Vacancies.Add(vacancy);
			}
		}
	}
}
