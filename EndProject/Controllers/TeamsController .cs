using EndProject.Services;
using FootballApiProject.Models.DTO_s.TeamsDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Threading.Tasks;

public class TeamsController : Controller
{
    private readonly ITeamService _teamService;
    private readonly IStadiumService _stadiumService;
    private readonly ILeagueService _leagueService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public TeamsController(ITeamService teamService, IStadiumService stadiumService, ILeagueService leagueService, IWebHostEnvironment hostingEnvironment)
    {
        _teamService = teamService;
        _stadiumService = stadiumService;
        _leagueService = leagueService;
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var teams = await _teamService.GetTeamsAsync();
        return View(teams);
    }

    public async Task<IActionResult> Create()
    {
        var stadiums = await _stadiumService.GetStadiumsAsync();
        var leagues = await _leagueService.GetLeaguesAsync();

        ViewBag.Stadiums = new SelectList(stadiums, "Name", "Name");
        ViewBag.Leagues = new SelectList(leagues, "Name", "Name");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTeamDto createTeamDto, IFormFile logo)
    {
        if (logo != null && logo.Length > 0)
        {
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "teams");
            Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(logo.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await logo.CopyToAsync(stream);
            }
            createTeamDto.LogoPath = "/images/teams/" + uniqueFileName;
        }

        if (ModelState.IsValid)
        {
            var success = await _teamService.CreateTeamAsync(createTeamDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        var stadiums = await _stadiumService.GetStadiumsAsync();
        var leagues = await _leagueService.GetLeaguesAsync();

        ViewBag.Stadiums = new SelectList(stadiums, "Name", "Name");
        ViewBag.Leagues = new SelectList(leagues, "Name", "Name");

        return View(createTeamDto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var team = await _teamService.GetTeamByIdAsync(id);
        if (team == null) return NotFound();

        var updateTeamDto = new UpdateTeamDto
        {
            TeamId = team.TeamId,
            Name = team.Name,
            Coach = team.Coach,
            StadiumName = team.StadiumName,
            LeagueName = team.LeagueName,
            LogoPath = team.LogoPath
        };

        return View(updateTeamDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateTeamDto updateTeamDto, IFormFile logo)
    {
        var existingTeam = await _teamService.GetTeamByIdAsync(id);
        if (existingTeam == null)
        {
            return NotFound();
        }

        if (logo != null && logo.Length > 0)
        {
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "teams");
            Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(logo.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await logo.CopyToAsync(stream);
            }

            // Delete the old logo file if a new one is uploaded
            if (!string.IsNullOrEmpty(existingTeam.LogoPath))
            {
                var oldLogoPath = Path.Combine(_hostingEnvironment.WebRootPath, existingTeam.LogoPath.TrimStart('/'));
                if (System.IO.File.Exists(oldLogoPath))
                {
                    System.IO.File.Delete(oldLogoPath);
                }
            }

            updateTeamDto.LogoPath = "/images/teams/" + uniqueFileName;
        }
        else
        {
            updateTeamDto.LogoPath = existingTeam.LogoPath;
        }

        if (ModelState.IsValid)
        {
            var success = await _teamService.UpdateTeamAsync(id, updateTeamDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        var stadiums = await _stadiumService.GetStadiumsAsync();
        var leagues = await _leagueService.GetLeaguesAsync();

        ViewBag.Stadiums = new SelectList(stadiums, "Name", "Name");
        ViewBag.Leagues = new SelectList(leagues, "Name", "Name");

        return View(updateTeamDto);
    }

    public async Task<IActionResult> Details(int id)
    {
        var team = await _teamService.GetTeamByIdAsync(id);
        if (team == null) return NotFound();

        var teamDetails = new TeamDetailsDto
        {
            TeamId = team.TeamId,
            Name = team.Name,
            Coach = team.Coach,
            LogoPath = team.LogoPath,
            StadiumName = team.StadiumName,
            LeagueName = team.LeagueName
        };

        return View(teamDetails);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var team = await _teamService.GetTeamByIdAsync(id);
        if (team == null) return NotFound();

        var teamDetails = new TeamDetailsDto
        {
            TeamId = team.TeamId,
            Name = team.Name,
            Coach = team.Coach,
            StadiumName = team.StadiumName,
            LeagueName = team.LeagueName,
            LogoPath = team.LogoPath
        };

        return View(teamDetails);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var team = await _teamService.GetTeamByIdAsync(id);
        if (team == null)
        {
            return NotFound();
        }

        var logoPath = Path.Combine(_hostingEnvironment.WebRootPath, team.LogoPath.TrimStart('/'));

        if (System.IO.File.Exists(logoPath))
        {
            System.IO.File.Delete(logoPath);
        }

        var success = await _teamService.DeleteTeamAsync(id);
        if (success)
        {
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View("Delete", team);
        }
    }
}
