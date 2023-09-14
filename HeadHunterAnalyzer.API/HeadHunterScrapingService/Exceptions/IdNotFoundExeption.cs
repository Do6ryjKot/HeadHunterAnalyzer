namespace HeadHunterScrapingService.Exceptions {

	/// <summary>
	/// Ошибка возникающая когда ид HH по какой-либи причине достать не получилось.
	/// </summary>
	public class IdNotFoundExeption : VacancyParsingException {

		public IdNotFoundExeption(string? message) : base(message) { }

		public IdNotFoundExeption(string? message, Exception? innerException) : base(message, innerException) { }
	}
}
