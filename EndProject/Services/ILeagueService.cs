using FootballApiProject.Models.DTO_s.LeaguesDto;

namespace EndProject.Services
{
    public interface ILeagueService
    {
        Task<IEnumerable<LeagueDto>> GetLeaguesAsync();
        Task<LeagueDetailsDto> GetLeagueByIdAsync(int id);
        Task<bool> CreateLeagueAsync(CreateLeagueDto createLeagueDto);
        Task<bool> UpdateLeagueAsync(int id, UpdateLeagueDto updateLeagueDto);
        Task<bool> DeleteLeagueAsync(int id);
    }
}
