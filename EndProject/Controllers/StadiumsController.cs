using EndProject.Services;
using FootballApiProject.Models.DTO_s.StadiumsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Threading.Tasks;

public class StadiumsController : Controller
{
    private readonly IStadiumService _stadiumService;

    public StadiumsController(IStadiumService stadiumService)
    {
        _stadiumService = stadiumService;
    }

    public async Task<IActionResult> Index()
    {
        var stadiums = await _stadiumService.GetStadiumsAsync();
        return View(stadiums);
    }

    // GET: Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateStadiumDto createStadiumDto, IFormFile image)
    {
        if (image != null && image.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/images/stadiums", Path.GetFileName(image.FileName));
            using (var stream = System.IO.File.Create(filePath))
            {
                await image.CopyToAsync(stream);
            }
            createStadiumDto.ImagePath = "/images/stadiums/" + Path.GetFileName(image.FileName);
        }

        if (ModelState.IsValid)
        {
            var success = await _stadiumService.CreateStadiumAsync(createStadiumDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        return View(createStadiumDto);
    }

    // GET: Edit
    public async Task<IActionResult> Edit(int id)
    {
        var stadium = await _stadiumService.GetStadiumByIdAsync(id);
        if (stadium == null) return NotFound();

        var updateStadiumDto = new UpdateStadiumDto
        {
            StadiumId = stadium.StadiumId,
            Name = stadium.Name,
            Location = stadium.Location,
            Capacity = stadium.Capacity,
            ImagePath = stadium.ImagePath
        };

        return View(updateStadiumDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateStadiumDto updateStadiumDto, IFormFile image)
    {
        var existingStadium = await _stadiumService.GetStadiumByIdAsync(id);
        if (existingStadium == null)
        {
            return NotFound();
        }

        if (image != null && image.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/images/stadiums", Path.GetFileName(image.FileName));
            using (var stream = System.IO.File.Create(filePath))
            {
                await image.CopyToAsync(stream);
            }
            updateStadiumDto.ImagePath = "/images/stadiums/" + Path.GetFileName(image.FileName);
        }
        else
        {
            updateStadiumDto.ImagePath = existingStadium.ImagePath;
        }

        if (ModelState.IsValid)
        {
            var success = await _stadiumService.UpdateStadiumAsync(id, updateStadiumDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        return View(updateStadiumDto);
    }

    // GET: Details
    public async Task<IActionResult> Details(int id)
    {
        var stadium = await _stadiumService.GetStadiumByIdAsync(id);
        if (stadium == null) return NotFound();

        return View(stadium);
    }

    // GET: Delete
    public async Task<IActionResult> Delete(int id)
    {
        var stadium = await _stadiumService.GetStadiumByIdAsync(id);
        if (stadium == null) return NotFound();

        return View(stadium);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var stadium = await _stadiumService.GetStadiumByIdAsync(id);
        if (stadium == null)
        {
            return NotFound();
        }

        var imagePath = Path.Combine("wwwroot", stadium.ImagePath.TrimStart('/'));
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }

        var success = await _stadiumService.DeleteStadiumAsync(id);
        if (success)
        {
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View(stadium);
        }
    }
}
