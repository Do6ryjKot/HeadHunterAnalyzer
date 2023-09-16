namespace HeadHunterScrapingService.Exceptions {

	/// <summary>
	/// Ошибка возникающая при отсутствии искомой ноды в спаршенном документе
	/// </summary>
	public class NodeNotFoundException : VacancyParsingException {

		public NodeNotFoundException(string? message, int headHunterId) : base(message, headHunterId) { }

		public NodeNotFoundException(string? message, Exception? innerException, int headHunterId) : base(message, headHunterId) { }
	}
}
