using Entities.Models;
using System.Diagnostics.CodeAnalysis;

namespace HeadHunterAnalyzer.API.Managers {
	
	/// <summary>
	/// Класс для сравнения слов по значению.
	/// </summary>
	public class WordComparer : IEqualityComparer<Word> {

		public bool Equals(Word? x, Word? y) {

			if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
				return false;

			if (Object.ReferenceEquals(x, y))
				return true;

			return x.Value == y.Value;
		}

		public int GetHashCode([DisallowNull] Word obj) =>
			obj.Value.GetHashCode();
	}
}
