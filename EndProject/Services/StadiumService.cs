using FootballApiProject.Models.DTO_s.StadiumsDto;
using System.Net.Http;
using System.Net.Http.Json;

namespace EndProject.Services
{
    public class StadiumService : IStadiumService
    {
        private readonly HttpClient _httpClient;

        public StadiumService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FootballApi");
        }

        public async Task<IEnumerable<StadiumDto>> GetStadiumsAsync()
        {
            var response = await _httpClient.GetAsync("StadiumsApi");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<StadiumDto>>();
        }

        public async Task<StadiumDetailsDto> GetStadiumByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"StadiumsApi/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StadiumDetailsDto>();
        }

        public async Task<bool> CreateStadiumAsync(CreateStadiumDto createStadiumDto)
        {
            var response = await _httpClient.PostAsJsonAsync("StadiumsApi", createStadiumDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateStadiumAsync(int id, UpdateStadiumDto updateStadiumDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"StadiumsApi/{id}", updateStadiumDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteStadiumAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"StadiumsApi/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
