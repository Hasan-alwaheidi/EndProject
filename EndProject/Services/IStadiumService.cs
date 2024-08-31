using FootballApiProject.Models.DTO_s.StadiumsDto;

namespace EndProject.Services
{
    public interface IStadiumService
    {
        Task<IEnumerable<StadiumDto>> GetStadiumsAsync();
        Task<StadiumDetailsDto> GetStadiumByIdAsync(int id);
        Task<bool> CreateStadiumAsync(CreateStadiumDto createStadiumDto);
        Task<bool> UpdateStadiumAsync(int id, UpdateStadiumDto updateStadiumDto);
        Task<bool> DeleteStadiumAsync(int id);
    }
}
