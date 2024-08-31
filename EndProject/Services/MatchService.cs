using FootballApiProject.Models.DTO_s.MatchsDto;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EndProject.Services
{
    public class MatchService : IMatchService
    {
        private readonly HttpClient _httpClient;

        public MatchService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FootballApi");
        }

        public async Task<IEnumerable<MatchDto>> GetMatchesAsync()
        {
            var response = await _httpClient.GetAsync("MatchesApi");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<MatchDto>>();
        }

        public async Task<MatchDetailsDto> GetMatchByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"MatchesApi/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MatchDetailsDto>();
        }

        public async Task<bool> CreateMatchAsync(CreateMatchDto createMatchDto)
        {
            var response = await _httpClient.PostAsJsonAsync("MatchesApi", createMatchDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateMatchAsync(int id, UpdateMatchDto updateMatchDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"MatchesApi/{id}", updateMatchDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMatchAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"MatchesApi/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<IEnumerable<MatchDto>> GetMatchesForDateAsync(DateTime date)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<MatchDto>>($"api/matches?date={date:yyyy-MM-dd}");
        }

        public async Task<IEnumerable<MatchDto>> GetPastMatchesAsync()
        {
            // Assuming your API has an endpoint for past matches
            return await _httpClient.GetFromJsonAsync<IEnumerable<MatchDto>>("api/matches/past");
        }

        public async Task<IEnumerable<MatchDto>> GetUpcomingMatchesAsync()
        {
            // Assuming your API has an endpoint for upcoming matches
            return await _httpClient.GetFromJsonAsync<IEnumerable<MatchDto>>("api/matches/upcoming");
        }
    }
}
