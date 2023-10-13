using Contracts.DataServices;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository {

	public class VacancyRepository : RepositoryBase<Vacancy>, IVacancyRepository {

		public VacancyRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }		

		public async Task<IEnumerable<Vacancy>> GetAllVacancies(bool trackChanges) => 
			await FindAll(trackChanges).ToListAsync();

		public async Task<Vacancy?> GetVacancyAsync(int headHunterId, bool trackChanges) =>
			await FindByCondition(vacancy => vacancy.HeadHunterId == headHunterId, trackChanges).FirstOrDefaultAsync();

		public void CreateVacancy(Vacancy vacancy) => Create(vacancy);
	}
}
