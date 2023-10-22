using Entities.DataTransferObjects;
using Entities.Models;

namespace HeadHunterAnalyzer.API.Managers {
	

	public interface IVacanciesManager {

		/// <summary>
		/// Сохраняет вакансию.
		/// </summary>
		/// <param name="vacancyDto">Данные вакансии</param>
		public Task<VacancyDto> SaveVacancyAsync(VacancyForCreationDto vacancyDto);

		/// <summary>
		/// Добавляет набор слов к уже созданной вакансии.
		/// </summary>
		/// <param name="vacancy">Вакансия к которой нужно добавить слова.</param>
		/// <param name="words">Слова, что нужно добавить вакансии.</param>
		/// <returns>Новый набор слов вакансии.</returns>
		public Task<IEnumerable<WordDto>> AddWordsToVacancyAsync(Vacancy vacancy, IEnumerable<WordForCreationDto> words);

		/// <summary>
		/// Возвращает даные по вакансии.
		/// </summary>
		/// <param name="headHunterId">Ид вакансии.</param>
		/// <returns>Данные вакансии.</returns>
		public Task<AnalyzedVacancyDto> AnalyzeVacancyAsync(int headHunterId);
	}
}
