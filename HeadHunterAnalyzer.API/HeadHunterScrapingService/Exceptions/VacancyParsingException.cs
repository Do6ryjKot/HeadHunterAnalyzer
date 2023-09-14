namespace HeadHunterScrapingService.Exceptions {

	/// <summary>
	/// Ошибка парсинга вакансии.
	/// </summary>
	public abstract class VacancyParsingException : Exception {

		public VacancyParsingException(string? message) : base(message) {
		}

		public VacancyParsingException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}
