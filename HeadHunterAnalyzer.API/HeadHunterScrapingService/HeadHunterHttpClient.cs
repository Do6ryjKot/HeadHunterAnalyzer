namespace HeadHunterScrapingService {
	
	public class HeadHunterHttpClient {

		private readonly HttpClient _client;

		public HeadHunterHttpClient(HttpClient client) {

			_client = client;
		}

		public async Task<Stream> GetVacancyData(int headHunterId) => 
			await _client.GetStreamAsync($"vacancy/{headHunterId}");
	}
}
