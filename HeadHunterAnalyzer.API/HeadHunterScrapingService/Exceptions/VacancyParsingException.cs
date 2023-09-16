namespace HeadHunterScrapingService.Exceptions {

	/// <summary>
	/// Ошибка парсинга вакансии.
	/// </summary>
	public abstract class VacancyParsingException : Exception {

		public int HeadHunterId { get; set; }

		public VacancyParsingException(string? message, int headHunterId) : base(message) {
			HeadHunterId = headHunterId;
		}

		public VacancyParsingException(string? message, Exception? innerException, int headHunterId) : base(message, innerException) {
			HeadHunterId = headHunterId;
		}
	}
}
