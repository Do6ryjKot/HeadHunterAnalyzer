using Entities.Models;
using Entities.RequestFeatures;

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

		public Task<Vacancy?> GetVacancyAsync(Guid id, bool trackChanges);

		public void CreateVacancy(Vacancy vacancy);

		/// <summary>
		/// Получить все анализированные вакансии.
		/// </summary>
		/// <param name="trackChanges"></param>
		/// <returns></returns>
		public Task<PagedList<Vacancy>> GetAllVacancies(RequestParameters parameters, bool trackChanges);
	}
}
