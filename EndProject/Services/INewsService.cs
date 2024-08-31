using EndProject.Models;
using SharedDtos.DTO_s.NewsDto;
namespace EndProject.Services
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDtoo>> GetNewsItemsAsync();
        Task<NewsDetailsDto> GetNewsItemByIdAsync(int id);
        Task<bool> CreateNewsItemAsync(CreateNewsDto createNewsDto);
        Task<bool> UpdateNewsItemAsync(int id, UpdateNewsDto updateNewsDto);
        Task<bool> DeleteNewsItemAsync(int id);
    }
}
