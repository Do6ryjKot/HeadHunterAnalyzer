using Contracts.DataServices;
using Entities;
using Entities.Models;

namespace Repository {

	public class VacancyRepository : RepositoryBase<Vacancy>, IVacancyRepository {

		public VacancyRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
	}
}
