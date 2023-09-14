using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects {
	
	/// <summary>
	/// DTO вакансии при сохранении.
	/// </summary>
	public class VacancyForCreationDto {

		[Required(ErrorMessage = "Ид вакансии необходимое поле.")]
		public int HeadHunterId { get; set; }

		[Required(ErrorMessage = "Набор слов обязательное поле.")]
		public IEnumerable<WordForCreationDto> Words { get; set; }
	}
}
