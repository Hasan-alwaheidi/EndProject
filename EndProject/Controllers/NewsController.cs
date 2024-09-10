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
    public async Task<IActionResult> Index()
    {
        var newsItems = await _newsService.GetNewsItemsAsync();
        var orderedNewsItems = newsItems.OrderByDescending(n => n.DatePublished).ToList();

        return View(orderedNewsItems);
    }

    // GET: Create
    public IActionResult Create()
    {
        return View();
    }
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
        createNewsDto.DatePublished = DateTime.Now;

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

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateNewsDto updateNewsDto, IFormFile image)
    {
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
            updateNewsDto.ImagePath = existingNews.ImagePath;
        }
        updateNewsDto.DatePublished = existingNews.DatePublished;

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
        var imagePath = Path.Combine("wwwroot", newsItem.ImagePath.TrimStart('/'));
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }
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
