namespace HeadHunterScrapingService.Exceptions {

	/// <summary>
	/// Вакансия находится в архиве HeadHunter.
	/// </summary>
	public class ArchivedVacancyException : Exception {

		public int HeadHunterId { get; }

		public ArchivedVacancyException(string? message, int headHunterId) : base(message) {
			HeadHunterId = headHunterId;
		}

		public ArchivedVacancyException(string? message, Exception? innerException, int headHunterId) : base(message, innerException) {
			HeadHunterId = headHunterId;
		}
	}
}
