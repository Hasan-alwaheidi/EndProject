using FootballApiProject.Models.DTO_s.PlayersDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EndProject.Services
{
    public interface IPlayerService
    {
        Task<IEnumerable<PlayerDto>> GetPlayersAsync();
        Task<PlayerDto> GetPlayerByIdAsync(int id);
        Task<bool> CreatePlayerAsync(CreatePlayerDto createPlayerDto);
        Task<bool> UpdatePlayerAsync(int id, UpdatePlayerDto updatePlayerDto);
        Task<bool> DeletePlayerAsync(int id);
        Task<PlayerDetailsDto> GetPlayerDetailsAsync(int id);
    }
}
