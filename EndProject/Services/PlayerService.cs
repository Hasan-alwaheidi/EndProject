using FootballApiProject.Models.DTO_s.PlayersDto;

namespace EndProject.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly HttpClient _httpClient;

        public PlayerService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FootballApi");
        }

        public async Task<IEnumerable<PlayerDto>> GetPlayersAsync()
        {
            var response = await _httpClient.GetAsync("PlayersApi");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<PlayerDto>>();
        }

        public async Task<PlayerDto> GetPlayerByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"PlayersApi/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PlayerDto>();
        }
        public async Task<PlayerDetailsDto> GetPlayerDetailsAsync(int id)
        {
            var response = await _httpClient.GetAsync($"PlayersApi/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PlayerDetailsDto>();
        }
        public async Task<bool> CreatePlayerAsync(CreatePlayerDto createPlayerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("PlayersApi", createPlayerDto);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"CreatePlayerAsync failed: {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePlayerAsync(int id, UpdatePlayerDto updatePlayerDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"PlayersApi/{id}", updatePlayerDto);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"UpdatePlayerAsync failed: {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePlayerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"PlayersApi/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
