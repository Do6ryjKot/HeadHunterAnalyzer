using Contracts.DataServices;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository {

	public class VacancyRepository : RepositoryBase<Vacancy>, IVacancyRepository {

		public VacancyRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

		public async Task<PagedList<Vacancy>> GetAllVacancies(RequestParameters parameters, bool trackChanges) {

			var query = FindAll(trackChanges)
				.Include(vacancy => vacancy.Words);

			var vacancies = await query
				.Skip((parameters.PageNumber - 1) * parameters.PageSize)
				.Take(parameters.PageSize)
				.ToListAsync();

			var count = await query.CountAsync();

			return PagedList<Vacancy>
				.ToPagedList(vacancies, count, parameters.PageNumber, parameters.PageSize);
		}

		public async Task<Vacancy?> GetVacancyAsync(int headHunterId, bool trackChanges) =>
			await FindByCondition(vacancy => vacancy.HeadHunterId == headHunterId, trackChanges).FirstOrDefaultAsync();

		public void CreateVacancy(Vacancy vacancy) => Create(vacancy);
	}
}
