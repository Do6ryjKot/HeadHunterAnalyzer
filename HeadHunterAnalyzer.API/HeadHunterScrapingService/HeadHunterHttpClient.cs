namespace HeadHunterScrapingService {
	
	public class HeadHunterHttpClient : HttpClient {

		public HeadHunterHttpClient() {

			BaseAddress = new Uri("https://hh.ru");
		}

		public async Task<Stream> GetVacancyData(int headHunterId) => 
			await GetStreamAsync($"vacancy/{headHunterId}");
	}
}
