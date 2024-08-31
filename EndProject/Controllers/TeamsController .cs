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
    public TeamsController(ITeamService teamService, IStadiumService stadiumService, ILeagueService leagueService)
    {
        _teamService = teamService;
        _stadiumService = stadiumService;
        _leagueService = leagueService;
    }

    // Index action to list all teams
    public async Task<IActionResult> Index()
    {
        var teams = await _teamService.GetTeamsAsync();
        return View(teams);
    }

    // GET: Create
    public async Task<IActionResult> Create()
    {
        // Assuming you have services for fetching stadiums and leagues
        var stadiums = await _stadiumService.GetStadiumsAsync();
        var leagues = await _leagueService.GetLeaguesAsync();

        // Populating ViewBag with SelectList items for dropdowns
        ViewBag.Stadiums = new SelectList(stadiums, "Name", "Name"); // Display Stadium Name
        ViewBag.Leagues = new SelectList(leagues, "Name", "Name");   // Display League Name

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTeamDto createTeamDto, IFormFile logo)
    {
        if (logo != null && logo.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/images", Path.GetFileName(logo.FileName));
            using (var stream = System.IO.File.Create(filePath))
            {
                await logo.CopyToAsync(stream);
            }
            createTeamDto.LogoPath = "/images/" + Path.GetFileName(logo.FileName);
        }

        if (ModelState.IsValid)
        {
            var success = await _teamService.CreateTeamAsync(createTeamDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // Repopulate ViewBag in case the ModelState is invalid and the view is re-displayed
        var stadiums = await _stadiumService.GetStadiumsAsync();
        var leagues = await _leagueService.GetLeaguesAsync();

        ViewBag.Stadiums = new SelectList(stadiums, "Name", "Name");
        ViewBag.Leagues = new SelectList(leagues, "Name", "Name");

        return View(createTeamDto);
    }


    // GET: Edit
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
        // First, load the existing team details
        var existingTeam = await _teamService.GetTeamByIdAsync(id);
        if (existingTeam == null)
        {
            return NotFound(); // Handle the case where the team doesn't exist
        }

        // If a new logo is uploaded, save it and update the logo path
        if (logo != null && logo.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/images", Path.GetFileName(logo.FileName));
            using (var stream = System.IO.File.Create(filePath))
            {
                await logo.CopyToAsync(stream);
            }
            updateTeamDto.LogoPath = "/images/" + Path.GetFileName(logo.FileName);
        }
        else
        {
            // If no new logo is uploaded, retain the existing logo path
            updateTeamDto.LogoPath = existingTeam.LogoPath;
        }

        // Validate and save the updated team details
        if (ModelState.IsValid)
        {
            var success = await _teamService.UpdateTeamAsync(id, updateTeamDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // Repopulate ViewBag in case the ModelState is invalid and the view is re-displayed
        var stadiums = await _stadiumService.GetStadiumsAsync();
        var leagues = await _leagueService.GetLeaguesAsync();

        ViewBag.Stadiums = new SelectList(stadiums, "Name", "Name");
        ViewBag.Leagues = new SelectList(leagues, "Name", "Name");

        return View(updateTeamDto);
    }

    // GET: Details
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

    // GET: Delete
    public async Task<IActionResult> Delete(int id)
    {
        // Fetch the detailed information of the team
        var team = await _teamService.GetTeamByIdAsync(id);
        if (team == null) return NotFound();

        // Map the TeamDto to TeamDetailsDto if necessary, or use the appropriate method to get TeamDetailsDto
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

    // POST: Delete
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Get the team details first to access the logo path
        var team = await _teamService.GetTeamByIdAsync(id);
        if (team == null)
        {
            return NotFound();
        }

        // Get the full path to the logo file
        var logoPath = Path.Combine("wwwroot", team.LogoPath.TrimStart('/'));

        // Check if the file exists and delete it
        if (System.IO.File.Exists(logoPath))
        {
            System.IO.File.Delete(logoPath);
        }

        // Proceed to delete the team from the database
        var success = await _teamService.DeleteTeamAsync(id);
        if (success)
        {
            return RedirectToAction(nameof(Index));
        }
        else
        {
            // If deletion fails, return the delete view again with the current team details
            return View("Delete", team);
        }
    }

}
