
namespace HeadHunterScrapingService.Exceptions {

	/// <summary>
	/// Ошибка при отсутствии определенного атрибута.
	/// </summary>
	public class AttributeNotFoundException : VacancyParsingException {

		public string AtributeName { get; }

		public AttributeNotFoundException(string? message, string atributeName) : base(message) {
			AtributeName = atributeName;
		}

		public AttributeNotFoundException(string? message, Exception? innerException, string atributeName) : base(message, innerException) {
			AtributeName = atributeName;
		}
	}
}
