
namespace HeadHunterScrapingService.Exceptions {

	/// <summary>
	/// Ошибка возникающая если спаршенная страница null.
	/// </summary>
	public class NullParsedDocumentException : VacancyParsingException {

		public NullParsedDocumentException(string? message) : base(message) {
		}

		public NullParsedDocumentException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}
