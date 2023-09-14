using System.ComponentModel.DataAnnotations;

namespace Entities.Models {
	
	public class Company {

		public Guid Id { get; set; }

		[Required(ErrorMessage = "HeadHunterId обязательное поле")]
		public int HeadHunterId { get; set; }

		[Required(ErrorMessage = "Название компании обязательное поле")]
		[MaxLength(100, ErrorMessage = "Название компании может содержать максимум 100 символов")]
		public string Name { get; set; }

		public ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
	}
}
