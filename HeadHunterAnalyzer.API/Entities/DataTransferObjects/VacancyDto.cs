namespace Entities.DataTransferObjects {
	
	public class VacancyDto {

		public Guid Id { get; set; }
		public int HeadHunterId { get; set; }
		public string Name { get; set; }

		public List<WordDto> Words { get; set; }
	}
}
