using Entities.DataTransferObjects;
using Entities.Models;
using System.Collections;

namespace Contracts.DataServices {

	public interface IWordRepository {

		public Task<IEnumerable<WordOccurrencesDto>> GetAllWordsOccurrences();

		public Task<IEnumerable<Word>> GetAllWords(bool trackChanges);

		public Task<IEnumerable<Word>> GetWordsByVacancyIdAsync(Guid vacancyId, bool trackChanges);

		public Task<IEnumerable<Word>> GetWordsByValuesAsync(IEnumerable<string> values, bool trackChanges);

		public void CreateWord(Word word);
	}
}
