using Contracts.DataServices;
using Entities;

namespace Repository {
	
	/// <summary>
	/// Класс содержащий ссылки на репозитории моделей
	/// </summary>
	public class RepositoryManager : IRepositoryManager {

		private readonly RepositoryContext _repositoryContext;
		private ICompanyRepository _companyRepository;
		private IVacancyRepository _vacancyRepository;
		private IWordRepository _wordRepository;

		public RepositoryManager(RepositoryContext repositoryContext) {

			_repositoryContext = repositoryContext;
		}

		public ICompanyRepository Companies { 
		
			get {

				if (_companyRepository == null)
					_companyRepository = new CompanyRepository(_repositoryContext);

				return _companyRepository;
			}
		}

		public IVacancyRepository Vacancies { 
			get {

				if (_vacancyRepository == null)
					_vacancyRepository = new VacancyRepository(_repositoryContext);

				return _vacancyRepository;
			} 
		}

		public IWordRepository Words { 
			
			get {

				if (_wordRepository == null)
					_wordRepository = new WordRepository(_repositoryContext);

				return _wordRepository;
			}
		}

		public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
	}
}
