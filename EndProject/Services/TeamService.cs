using FootballApiProject.Models.DTO_s;
using FootballApiProject.Models.DTO_s.TeamsDto;

namespace EndProject.Services
{
    public class TeamService : ITeamService
    {
        private readonly HttpClient _httpClient;

        public TeamService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FootballApi");
        }

        public async Task<IEnumerable<TeamDto>> GetTeamsAsync()
        {
            var response = await _httpClient.GetAsync("TeamsApi");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<TeamDto>>();
        }

        public async Task<TeamDto> GetTeamByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"TeamsApi/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TeamDto>();
        }

        public async Task<bool> CreateTeamAsync(CreateTeamDto createTeamDto)
        {
            var response = await _httpClient.PostAsJsonAsync("TeamsApi", createTeamDto);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"CreateTeamAsync failed: {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTeamAsync(int id, UpdateTeamDto updateTeamDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"TeamsApi/{id}", updateTeamDto);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"UpdateTeamAsync failed: {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"TeamsApi/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
