
namespace HeadHunterScrapingService.Exceptions {

	/// <summary>
	/// Ошибка возникающая если спаршенная страница null.
	/// </summary>
	public class NullParsedDocumentException : VacancyParsingException {

		public NullParsedDocumentException(string? message, int headHunterId) : base(message, headHunterId) {
		}

		public NullParsedDocumentException(string? message, Exception? innerException, int headHunterId) : base(message, headHunterId) {
		}
	}
}
