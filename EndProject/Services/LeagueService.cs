using FootballApiProject.Models.DTO_s.LeaguesDto;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EndProject.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly HttpClient _httpClient;

        public LeagueService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FootballApi");
        }

        public async Task<IEnumerable<LeagueDto>> GetLeaguesAsync()
        {
            var response = await _httpClient.GetAsync("LeaguesApi");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<LeagueDto>>();
        }

        public async Task<LeagueDetailsDto> GetLeagueByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"LeaguesApi/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<LeagueDetailsDto>();
        }

        public async Task<bool> CreateLeagueAsync(CreateLeagueDto createLeagueDto)
        {
            var response = await _httpClient.PostAsJsonAsync("LeaguesApi", createLeagueDto);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"CreateLeagueAsync failed: {errorContent}");
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateLeagueAsync(int id, UpdateLeagueDto updateLeagueDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"LeaguesApi/{id}", updateLeagueDto);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"UpdateLeagueAsync failed: {errorContent}");
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteLeagueAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"LeaguesApi/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
