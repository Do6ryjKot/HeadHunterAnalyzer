namespace HeadHunterScrapingService.Exceptions {

	/// <summary>
	/// Ошибка возникающая при отсутствии искомой ноды в спаршенном документе
	/// </summary>
	public class NodeNotFoundException : VacancyParsingException {

		public NodeNotFoundException(string? message) : base(message) { }

		public NodeNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }
	}
}
