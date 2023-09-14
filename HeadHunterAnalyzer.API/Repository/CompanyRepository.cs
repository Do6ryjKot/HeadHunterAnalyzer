using Contracts.DataServices;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository {

	public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository {

		public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

		public void CreateCompany(Company company) => Create(company);

		public async Task<Company?> GetCompanyByHhIdAsync(int headHunterId, bool trackChanges) =>
			await FindByCondition(c => c.HeadHunterId == headHunterId, trackChanges).FirstOrDefaultAsync();
	}
}
