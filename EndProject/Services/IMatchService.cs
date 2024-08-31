using FootballApiProject.Models.DTO_s.MatchsDto;

namespace EndProject.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDto>> GetMatchesAsync();
        Task<MatchDetailsDto> GetMatchByIdAsync(int id);
        Task<bool> CreateMatchAsync(CreateMatchDto createMatchDto);
        Task<bool> UpdateMatchAsync(int id, UpdateMatchDto updateMatchDto);
        Task<bool> DeleteMatchAsync(int id);
    }
}
