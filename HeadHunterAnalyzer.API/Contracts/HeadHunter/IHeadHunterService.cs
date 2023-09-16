using Entities.DataTransferObjects;
using Entities.Models;

namespace Contracts.HeadHunter {
	
	/// <summary>
	/// Описание сервиса взаимодействия с hh.ru.
	/// </summary>
	public interface IHeadHunterService {

		public Task LoadVacancyAsync(int headHunterId);
		public Company GetCompany();
		public Vacancy GetVacancy();
		public VacancyData GetVacancyData();
	}
}
