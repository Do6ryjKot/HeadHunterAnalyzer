﻿using Contracts.DataServices;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository {

	public class WordRepository : RepositoryBase<Word>, IWordRepository {

		public WordRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

		public void CreateWord(Word word) => Create(word);

		public async Task<IEnumerable<Word>> GetAllWords(bool trackChanges) => await FindAll(trackChanges).ToListAsync();

		public async Task<IEnumerable<Word>> GetWordsByVacancyIdAsync(Guid vacancyId, bool trackChanges) =>
			await FindByCondition(word => word.Vacancies.Any(vacancy => vacancy.Id == vacancyId), trackChanges).ToListAsync();

		public async Task<IEnumerable<Word>> GetWordsByValuesAsync(IEnumerable<string> values, bool trackChanges) {

			var lowerCaseValues = values.Select(value => value.ToLower());

			return await FindByCondition(word => lowerCaseValues.Contains(word.Value.ToLower()), trackChanges).ToListAsync();
		}

		public async Task<IEnumerable<WordOccurrencesDto>> GetAllWordsOccurrences() =>
			await FindAll(false).Select(word => 
				new WordOccurrencesDto { Id = word.Id, Value = word.Value, Occurrences = word.Vacancies.Count() }).ToListAsync();
	}
}
