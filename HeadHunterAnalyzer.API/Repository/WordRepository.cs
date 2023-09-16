using Contracts.DataServices;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository {

	public class WordRepository : RepositoryBase<Word>, IWordRepository {

		public WordRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

		public void CreateWord(Word word) => Create(word);

		public async Task<IEnumerable<Word>> GetWordsByVacancyIdAsync(Guid vacancyId, bool trackChanges) =>
			await FindByCondition(word => word.Vacancies.Any(vacancy => vacancy.Id == vacancyId), trackChanges).ToListAsync();

		public async Task<IEnumerable<Word>> GetWordsByValuesAsync(IEnumerable<string> values, bool trackChanges) =>
			await FindByCondition(word => values.Contains(word.Value), trackChanges).ToListAsync();
	}
}
