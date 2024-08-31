using FootballApiProject.Models.DTO_s;
using FootballApiProject.Models.DTO_s.TeamsDto;

namespace EndProject.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamDto>> GetTeamsAsync();
        Task<TeamDto> GetTeamByIdAsync(int id);
        Task<bool> CreateTeamAsync(CreateTeamDto createTeamDto);
        Task<bool> UpdateTeamAsync(int id, UpdateTeamDto updateTeamDto);
        Task<bool> DeleteTeamAsync(int id);
    }
}
