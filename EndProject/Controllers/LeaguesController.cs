using EndProject.Services;
using FootballApiProject.Models.DTO_s.LeaguesDto;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

public class LeaguesController : Controller
{
    private readonly ILeagueService _leagueService;

    public LeaguesController(ILeagueService leagueService)
    {
        _leagueService = leagueService;
    }

    public async Task<IActionResult> Index()
    {
        var leagues = await _leagueService.GetLeaguesAsync();
        return View(leagues);
    }

    public async Task<IActionResult> Details(int id)
    {
        var league = await _leagueService.GetLeagueByIdAsync(id);
        if (league == null) return NotFound();
        return View(league);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLeagueDto createLeagueDto, IFormFile logo)
    {
        if (logo != null && logo.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/images/leagues", Path.GetFileName(logo.FileName));
            using (var stream = System.IO.File.Create(filePath))
            {
                await logo.CopyToAsync(stream);
            }
            createLeagueDto.LogoPath = "/images/leagues/" + Path.GetFileName(logo.FileName);
        }

        if (ModelState.IsValid)
        {
            var success = await _leagueService.CreateLeagueAsync(createLeagueDto);
            if (success) return RedirectToAction(nameof(Index));
        }
        return View(createLeagueDto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var league = await _leagueService.GetLeagueByIdAsync(id);
        if (league == null) return NotFound();

        var updateLeagueDto = new UpdateLeagueDto
        {
            LeagueId = league.LeagueId,
            Name = league.Name,
            Country = league.Country,
            Season = league.Season,
            LogoPath = league.LogoPath,
            Description = league.Description
        };

        return View(updateLeagueDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateLeagueDto updateLeagueDto, IFormFile logo)
    {
        var existingLeague = await _leagueService.GetLeagueByIdAsync(id);
        if (existingLeague == null)
        {
            return NotFound();
        }

        if (logo != null && logo.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/images/leagues", Path.GetFileName(logo.FileName));
            using (var stream = System.IO.File.Create(filePath))
            {
                await logo.CopyToAsync(stream);
            }
            updateLeagueDto.LogoPath = "/images/leagues/" + Path.GetFileName(logo.FileName);
        }
        else
        {
            updateLeagueDto.LogoPath = existingLeague.LogoPath;
        }

        if (ModelState.IsValid)
        {
            var success = await _leagueService.UpdateLeagueAsync(id, updateLeagueDto);
            if (success) return RedirectToAction(nameof(Index));
        }
        return View(updateLeagueDto);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var league = await _leagueService.GetLeagueByIdAsync(id);
        if (league == null) return NotFound();
        return View(league);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var league = await _leagueService.GetLeagueByIdAsync(id);
        if (league == null)
        {
            return NotFound();
        }

        var logoPath = Path.Combine("wwwroot", league.LogoPath.TrimStart('/'));
        if (System.IO.File.Exists(logoPath))
        {
            System.IO.File.Delete(logoPath);
        }

        var success = await _leagueService.DeleteLeagueAsync(id);
        if (success)
        {
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View("Delete", league);
        }
    }
}
