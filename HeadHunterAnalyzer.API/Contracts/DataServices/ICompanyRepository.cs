using Entities.Models;

namespace Contracts.DataServices {

	public interface ICompanyRepository {

		public Task<Company?> GetCompanyByHhIdAsync(int headHunterId, bool trackChanges);

		public void CreateCompany(Company company);
	}
}
