using Entities.Models;

namespace Contracts.DataServices {

	/// <summary>
	/// Репозиторий вакансий.
	/// </summary>
	public interface IVacancyRepository {

		/// <summary>
		/// Получение вакансии по её ид на HH.
		/// </summary>
		/// <param name="headHunterId"></param>
		/// <param name="trackChanges"></param>
		/// <returns></returns>
		public Task<Vacancy?> GetVacancyAsync(int headHunterId, bool trackChanges);

		public void CreateVacancy(Vacancy vacancy);

		/// <summary>
		/// Получить все анализированные вакансии.
		/// </summary>
		/// <param name="trackChanges"></param>
		/// <returns></returns>
		public Task<IEnumerable<Vacancy>> GetAllVacancies(bool trackChanges);
	}
}
