using EndProject.Services;
using FootballApiProject.Models.DTO_s.MatchsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class MatchesController : Controller
{
    private readonly IMatchService _matchService;
    private readonly ITeamService _teamService;

    public MatchesController(IMatchService matchService, ITeamService teamService)
    {
        _matchService = matchService;
        _teamService = teamService;
    }

    // Index action to list all matches
    public async Task<IActionResult> Index()
    {
        var matches = await _matchService.GetMatchesAsync();
        return View(matches);
    }

    // GET: Create
    public async Task<IActionResult> Create()
    {
        var teams = await _teamService.GetTeamsAsync();
        ViewBag.Teams = new SelectList(teams, "TeamId", "Name");
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateMatchDto createMatchDto)
    {
        if (createMatchDto.Date > DateTime.Now)
        {
            ModelState.Remove("Score");
            ModelState.Remove("Result");
            createMatchDto.Score = null;
            createMatchDto.Result = null;
        }

        if (ModelState.IsValid)
        {
            var success = await _matchService.CreateMatchAsync(createMatchDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        var teams = await _teamService.GetTeamsAsync();
        ViewBag.Teams = new SelectList(teams, "TeamId", "Name");

        return View(createMatchDto);
    }

    // GET: Edit
    public async Task<IActionResult> Edit(int id)
    {
        var match = await _matchService.GetMatchByIdAsync(id);
        if (match == null)
        {
            return NotFound();
        }

        var teams = await _teamService.GetTeamsAsync();
        ViewBag.Teams = new SelectList(teams, "TeamId", "Name");

        return View(new UpdateMatchDto
        {
            MatchId = match.MatchId,
            HomeTeamId = match.HomeTeamId,
            AwayTeamId = match.AwayTeamId,
            Date = match.Date,
            Score = match.Score,
            Result = match.Result
        });
    }

    // POST: Edit
    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateMatchDto updateMatchDto)
    {
        if (ModelState.IsValid)
        {
            var success = await _matchService.UpdateMatchAsync(id, updateMatchDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        var teams = await _teamService.GetTeamsAsync();
        ViewBag.Teams = new SelectList(teams, "TeamId", "Name");

        return View(updateMatchDto);
    }

    // GET: Delete
    public async Task<IActionResult> Delete(int id)
    {
        var match = await _matchService.GetMatchByIdAsync(id);
        if (match == null)
        {
            return NotFound();
        }
        return View(match);
    }

    // POST: Delete
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var success = await _matchService.DeleteMatchAsync(id);
        if (success)
        {
            return RedirectToAction(nameof(Index));
        }

        var match = await _matchService.GetMatchByIdAsync(id);
        return View("Delete", match);
    }

    // GET: Details
    public async Task<IActionResult> Details(int id)
    {
        var match = await _matchService.GetMatchByIdAsync(id);
        if (match == null)
        {
            return NotFound();
        }
        return View(match);
    }
}
