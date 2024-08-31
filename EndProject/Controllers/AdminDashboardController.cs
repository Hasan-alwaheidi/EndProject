using EndProject.Models;
using EndProject.Services;
using Microsoft.AspNetCore.Mvc;
using SharedDtos.DTO_s.NewsDto;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class AdminDashboardController : Controller
{
    private readonly INewsService _newsService;

    public AdminDashboardController(INewsService newsService)
    {
        _newsService = newsService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string password)
    {
        if (!string.IsNullOrEmpty(password))
        {
            return RedirectToAction("Dashboard");
        }
        else
        {
            ViewBag.Error = "Password is required.";
            return View();
        }
    }

    public async Task<IActionResult> Dashboard()
    {
        var newsItems = await _newsService.GetNewsItemsAsync();

        var newsDetails = newsItems.Select(newsItem => new NewsDtoo
        {
            Id = newsItem.Id,
            Title = newsItem.Title,
            Content = newsItem.Content,
            DatePublished = newsItem.DatePublished,
            ImagePath = newsItem.ImagePath
        }).ToList();

        var model = new AdminDashboardViewModel
        {
            NewsItems = newsDetails
        };

        return View(model);
    }

    // GET: AdminDashboard/Create
    public IActionResult Create()
    {
        return View(new CreateNewsViewModel());
    }

    // POST: AdminDashboard/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateNewsViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var createNewsDto = new CreateNewsDto
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                ImagePath = await SaveImageAsync(viewModel.Image) // Saving the image and returning the path
            };

            var success = await _newsService.CreateNewsItemAsync(createNewsDto);
            if (success)
            {
                return RedirectToAction(nameof(Dashboard));
            }
        }

        return View(viewModel);
    }

    // GET: AdminDashboard/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var newsItem = await _newsService.GetNewsItemByIdAsync(id);
        if (newsItem == null)
        {
            return NotFound();
        }

        var viewModel = new EditNewsViewModel
        {
            Id = newsItem.Id,
            Title = newsItem.Title,
            Content = newsItem.Content,
            ImagePath = newsItem.ImagePath
        };

        return View(viewModel);
    }

    // POST: AdminDashboard/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditNewsViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var updateNewsDto = new UpdateNewsDto
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Content = viewModel.Content,
                ImagePath = string.IsNullOrEmpty(viewModel.ImagePath) ? await SaveImageAsync(viewModel.Image) : viewModel.ImagePath
            };

            // Assuming UpdateNewsItemAsync method signature is Task<bool> UpdateNewsItemAsync(int id, UpdateNewsDto updateNewsDto)
            var success = await _newsService.UpdateNewsItemAsync(id, updateNewsDto);
            if (success)
            {
                return RedirectToAction(nameof(Dashboard));
            }
        }

        return View(viewModel);
    }

    // GET: AdminDashboard/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var newsItem = await _newsService.GetNewsItemByIdAsync(id);
        if (newsItem == null)
        {
            return NotFound();
        }

        // Directly use the NewsDetailsDto returned by the service
        return View(newsItem);
    }

    // POST: AdminDashboard/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var success = await _newsService.DeleteNewsItemAsync(id);
        if (!success)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Dashboard));
    }

    private async Task<string> SaveImageAsync(IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return "/images/Default.jpg"; // Return a default image path if no image is uploaded
        }

        string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "newsimages");
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
        string filePath = Path.Combine(uploadFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        return "/newsimages/" + uniqueFileName;
    }
}
