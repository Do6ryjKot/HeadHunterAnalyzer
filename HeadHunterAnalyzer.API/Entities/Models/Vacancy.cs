using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models {

	public class Vacancy {

		public Guid Id { get; set; }

		[Required(ErrorMessage = "HeadHunterId обязательное поле")]
		public int HeadHunterId { get; set; }

		[Required(ErrorMessage = "Название вакансии (Name) обязательное поле")]
		[MaxLength(150, ErrorMessage = "Название вакансии (Name) может содержать максимум 150 символов")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Ид компании(CompanyId) вакансии обязательное поле")]
		[ForeignKey(nameof(Company))]
		public Guid CompanyId { get; set; }
		public Company Company { get; set; }

		public ICollection<Word> Words { get; set; } = new List<Word>();
	}
}
