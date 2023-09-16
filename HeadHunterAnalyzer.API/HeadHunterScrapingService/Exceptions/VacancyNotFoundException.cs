namespace HeadHunterScrapingService.Exceptions {
	
	/// <summary>
	/// Ошибка когда вакансия не найдена
	/// </summary>
	public class VacancyNotFoundException : VacancyParsingException {

		public VacancyNotFoundException(string? message, int headHunterId) : base(message, headHunterId) {
		}
	}
}
