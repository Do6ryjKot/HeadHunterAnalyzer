using Contracts.DataServices;
using Entities;
using Entities.Models;

namespace Repository {

	public class WordRepository : RepositoryBase<Word>, IWordRepository {

		public WordRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
	}
}
