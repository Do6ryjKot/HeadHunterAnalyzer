namespace Contracts.DataServices {
	
	/// <summary>
	/// Интерфейс для класса, что содержит репозитории для всех моделей.
	/// </summary>
	public interface IRepositoryManager {

		public ICompanyRepository Companies { get; }
		public IVacancyRepository Vacancies { get; }
		public IWordRepository Words { get; }

		public Task SaveAsync();
	}
}
