using EndProject.Models;
using EndProject.Services;
using Microsoft.AspNetCore.Mvc;
using SharedDtos.DTO_s.NewsDto;
using System.Linq;
using System.Threading.Tasks;

public class AdminDashboardController : Controller
{
    private readonly INewsService _newsService;
    private readonly IConfiguration _configuration;
    public AdminDashboardController(INewsService newsService, IConfiguration configuration)
    {
        _newsService = newsService;
       _configuration = configuration;
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            ViewBag.Error = "Password is required.";
            return View();
        }

        var storedHashedPassword = _configuration["AdminPasswordHash"];
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, storedHashedPassword);

        if (isPasswordValid)
        {
            HttpContext.Session.SetString("AdminAuthenticated", "true");
            return RedirectToAction("Dashboard");
        }
        else
        {
            ViewBag.Error = "Invalid password.";
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
    public IActionResult Create()
    {
        return View(new CreateNewsViewModel());
    }

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
                ImagePath = await SaveImageAsync(viewModel.Image) 
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

            var success = await _newsService.UpdateNewsItemAsync(id, updateNewsDto);
            if (success)
            {
                return RedirectToAction(nameof(Dashboard));
            }
        }

        return View(viewModel);
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
            return "/images/Default.jpg";
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
