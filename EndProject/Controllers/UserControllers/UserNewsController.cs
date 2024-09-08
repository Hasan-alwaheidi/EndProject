using EndProject.Services;
using Microsoft.AspNetCore.Mvc;
using SharedDtos.DTO_s.NewsDto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndProject.Controllers.UserControllers
{
    public class UserNewsController : Controller
    {
        private readonly INewsService _newsService;

        public UserNewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var newsItems = await _newsService.GetNewsItemsAsync();

            // Order news by DatePublished descending so the newest news appears first
            var newsDetailsList = newsItems
                .OrderByDescending(newsItem => newsItem.DatePublished) // Ordering by DatePublished
                .Select(newsItem => new NewsDetailsDto
                {
                    Id = newsItem.Id,
                    Title = newsItem.Title,
                    Content = newsItem.Content,
                    DatePublished = newsItem.DatePublished,
                    ImagePath = newsItem.ImagePath
                    // Map other necessary properties here
                })
                .ToList();

            return View(newsDetailsList);
        }


        public async Task<IActionResult> Details(int id)
        {
            var newsItem = await _newsService.GetNewsItemByIdAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }

            // Map NewsDtoo to NewsDetailsDto
            var newsDetails = new NewsDetailsDto
            {
                Id = newsItem.Id,
                Title = newsItem.Title,
                Content = newsItem.Content,
                DatePublished = newsItem.DatePublished,
                ImagePath = newsItem.ImagePath
                // Map other necessary properties here
            };

            return View(newsDetails);
        }
    }
}
