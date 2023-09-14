using Contracts.DataServices;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository {

	public class VacancyRepository : RepositoryBase<Vacancy>, IVacancyRepository {

		public VacancyRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

		public void CreateVacancy(Vacancy vacancy) => Create(vacancy);

		public async Task<Vacancy?> GetVacancyAsync(int headHunterId, bool trackChanges) =>
			await FindByCondition(vacancy => vacancy.HeadHunterId == headHunterId, trackChanges).FirstOrDefaultAsync();
	}
}
