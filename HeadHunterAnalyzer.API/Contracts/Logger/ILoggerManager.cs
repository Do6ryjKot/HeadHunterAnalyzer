
namespace Contracts.Logger {
	
	/// <summary>
	/// Интерфейс описывающий сейрвис логирования.
	/// </summary>
	public interface ILoggerManager {

		public void LogInformation(string message);
		public void LogWarning(string message);
		public void LogDebug(string message);
		public void LogError(string message);
	}
}
