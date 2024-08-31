using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using SharedDtos.DTO_s.NewsDto;

namespace EndProject.Services
{
    public class NewsService : INewsService
    {
        private readonly HttpClient _httpClient;

        public NewsService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FootballApi");
        }

        public async Task<IEnumerable<NewsDtoo>> GetNewsItemsAsync()
        {
            var response = await _httpClient.GetAsync("NewsApi");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<NewsDtoo>>();
        }

        public async Task<NewsDetailsDto> GetNewsItemByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"NewsApi/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<NewsDetailsDto>();
        }

        public async Task<bool> CreateNewsItemAsync(CreateNewsDto createNewsDto)
        {
            var response = await _httpClient.PostAsJsonAsync("NewsApi", createNewsDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateNewsItemAsync(int id, UpdateNewsDto updateNewsDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"NewsApi/{id}", updateNewsDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteNewsItemAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"NewsApi/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
