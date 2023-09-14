using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects {
	
	public class WordForCreationDto {

		[Required(ErrorMessage = "Значение слова обязательное поле")]
		[MaxLength(100, ErrorMessage = "Слово может содержать максимум 100 символов")]
		public string Value { get; set; }
	}
}
