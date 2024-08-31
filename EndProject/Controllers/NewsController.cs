using EndProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharedDtos.DTO_s.NewsDto;
using System.IO;
using System.Threading.Tasks;

public class NewsController : Controller
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    // Index action to list all news items
    public async Task<IActionResult> Index()
    {
        var newsItems = await _newsService.GetNewsItemsAsync();
        return View(newsItems);
    }

    // GET: Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Create
    [HttpPost]
    public async Task<IActionResult> Create(CreateNewsDto createNewsDto, IFormFile image)
    {
        if (image != null && image.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/newsimages", Path.GetFileName(image.FileName));
            using (var stream = System.IO.File.Create(filePath))
            {
                await image.CopyToAsync(stream);
            }
            createNewsDto.ImagePath = "/newsimages/" + Path.GetFileName(image.FileName);
        }

        if (ModelState.IsValid)
        {
            var success = await _newsService.CreateNewsItemAsync(createNewsDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        return View(createNewsDto);
    }

    // GET: Edit
    public async Task<IActionResult> Edit(int id)
    {
        var newsItem = await _newsService.GetNewsItemByIdAsync(id);
        if (newsItem == null)
        {
            return NotFound();
        }

        var updateNewsDto = new UpdateNewsDto
        {
            Id = newsItem.Id,
            Title = newsItem.Title,
            Content = newsItem.Content,
            ImagePath = newsItem.ImagePath,
            DatePublished = newsItem.DatePublished
        };

        return View(updateNewsDto);
    }

    // POST: Edit
    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateNewsDto updateNewsDto, IFormFile image)
    {
        // Get the existing news item to retrieve the current image path
        var existingNews = await _newsService.GetNewsItemByIdAsync(id);
        if (existingNews == null)
        {
            return NotFound();
        }

        if (image != null && image.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/newsimages", Path.GetFileName(image.FileName));
            using (var stream = System.IO.File.Create(filePath))
            {
                await image.CopyToAsync(stream);
            }
            updateNewsDto.ImagePath = "/newsimages/" + Path.GetFileName(image.FileName);
        }
        else
        {
            // Retain the existing image if no new image is uploaded
            updateNewsDto.ImagePath = existingNews.ImagePath;
        }

        if (ModelState.IsValid)
        {
            var success = await _newsService.UpdateNewsItemAsync(id, updateNewsDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        return View(updateNewsDto);
    }

    // GET: Delete
    public async Task<IActionResult> Delete(int id)
    {
        var newsItem = await _newsService.GetNewsItemByIdAsync(id);
        if (newsItem == null)
        {
            return NotFound();
        }
        return View(newsItem);
    }

    // POST: Delete
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var newsItem = await _newsService.GetNewsItemByIdAsync(id);
        if (newsItem == null)
        {
            return NotFound();
        }

        // Get the full path to the image file
        var imagePath = Path.Combine("wwwroot", newsItem.ImagePath.TrimStart('/'));

        // Check if the file exists and delete it
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }

        // Proceed to delete the news item from the database
        var success = await _newsService.DeleteNewsItemAsync(id);
        if (success)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(newsItem);
    }

    // GET: Details
    public async Task<IActionResult> Details(int id)
    {
        var newsItem = await _newsService.GetNewsItemByIdAsync(id);
        if (newsItem == null)
        {
            return NotFound();
        }
        return View(newsItem);
    }
}
