
namespace HeadHunterScrapingService.Exceptions {

	/// <summary>
	/// Ошибка при отсутствии определенного атрибута.
	/// </summary>
	public class AttributeNotFoundException : VacancyParsingException {

		public string AtributeName { get; }

		public AttributeNotFoundException(string? message, string atributeName, int headHunterId) : base(message, headHunterId) {
			AtributeName = atributeName;
		}

		public AttributeNotFoundException(string? message, Exception? innerException, string atributeName, int headHunterId) : base(message, headHunterId) {
			AtributeName = atributeName;
		}
	}
}
