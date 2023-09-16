using System.ComponentModel.DataAnnotations;

namespace Entities.Models {
	
	public class Word {

		public Guid Id { get; set; }

		[Required(ErrorMessage = "Значение слова обязательное поле")]
		[MaxLength(100, ErrorMessage = "Слово может содержать максимум 100 символов")]
		public string Value { get; set; }

		public ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
	}
}
