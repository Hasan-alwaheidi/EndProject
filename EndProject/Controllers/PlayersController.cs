using EndProject.Services;
using FootballApiProject.Enums;
using FootballApiProject.Models.DTO_s.PlayersDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;

public class PlayersController : Controller
{
    private readonly IPlayerService _playerService;
    private readonly ITeamService _teamService;

    public PlayersController(IPlayerService playerService, ITeamService teamService)
    {
        _playerService = playerService;
        _teamService = teamService;
    }
    public async Task<IActionResult> Index()
    {
        var players = await _playerService.GetPlayersAsync();
        return View(players);
    }

    // GET: Create
    public async Task<IActionResult> Create()
    {
        var teams = await _teamService.GetTeamsAsync();

        ViewBag.Teams = new SelectList(teams, "TeamId", "Name");
        ViewBag.Positions = Enum.GetValues(typeof(PlayerPosition))
            .Cast<PlayerPosition>()
            .Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePlayerDto createPlayerDto, IFormFile profilePicture)
    {
        if (profilePicture != null && profilePicture.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/images/Players", Path.GetFileName(profilePicture.FileName));
            using (var stream = System.IO.File.Create(filePath))
            {
                await profilePicture.CopyToAsync(stream);
            }
            createPlayerDto.ProfilePicturePath = "/images/Players/" + Path.GetFileName(profilePicture.FileName);
        }

        if (ModelState.IsValid)
        {
            var success = await _playerService.CreatePlayerAsync(createPlayerDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        var teams = await _teamService.GetTeamsAsync();
        ViewBag.Teams = new SelectList(teams, "TeamId", "Name");
        ViewBag.Positions = Enum.GetValues(typeof(PlayerPosition))
            .Cast<PlayerPosition>()
            .Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

        return View(createPlayerDto);
    }

    // GET: Edit
    public async Task<IActionResult> Edit(int id)
    {
        var player = await _playerService.GetPlayerByIdAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        var updatePlayerDto = new UpdatePlayerDto
        {
            PlayerId = player.PlayerId,
            Name = player.Name,
            Position = player.Position,
            Nationality = player.Nationality,
            TeamId = player.TeamId,
            ProfilePicturePath = player.ProfilePicturePath,
            Description = player.Description
        };

        var teams = await _teamService.GetTeamsAsync();
        ViewBag.Teams = new SelectList(teams, "TeamId", "Name");
        ViewBag.Positions = Enum.GetValues(typeof(PlayerPosition)).Cast<PlayerPosition>().ToList();

        return View(updatePlayerDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdatePlayerDto updatePlayerDto, IFormFile profilePicture)
    {
        var existingPlayer = await _playerService.GetPlayerByIdAsync(id);
        if (existingPlayer == null)
        {
            return NotFound();
        }

        if (profilePicture != null && profilePicture.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/images/Players", Path.GetFileName(profilePicture.FileName));
            using (var stream = System.IO.File.Create(filePath))
            {
                await profilePicture.CopyToAsync(stream);
            }
            updatePlayerDto.ProfilePicturePath = "/images/Players/" + Path.GetFileName(profilePicture.FileName);
        }
        else
        {
            updatePlayerDto.ProfilePicturePath = existingPlayer.ProfilePicturePath;
        }

        if (ModelState.IsValid)
        {
            var success = await _playerService.UpdatePlayerAsync(id, updatePlayerDto);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        var teams = await _teamService.GetTeamsAsync();
        ViewBag.Teams = new SelectList(teams, "TeamId", "Name");

        return View(updatePlayerDto);
    }
    // GET: Details
    public async Task<IActionResult> Details(int id)
    {
        var player = await _playerService.GetPlayerByIdAsync(id);
        if (player == null) return NotFound();

        var playerDetails = new PlayerDetailsDto
        {
            PlayerId = player.PlayerId,
            Name = player.Name,
            Position = player.Position,
            Nationality = player.Nationality,
            TeamName = player.TeamName,
            ProfilePicturePath = player.ProfilePicturePath,
            Description = player.Description
        };

        return View(playerDetails);
    }

    // GET: Delete
    public async Task<IActionResult> Delete(int id)
    {
        var player = await _playerService.GetPlayerByIdAsync(id);
        if (player == null) return NotFound();

        var playerDetails = new PlayerDetailsDto
        {
            PlayerId = player.PlayerId,
            Name = player.Name,
            Position = player.Position,
            Nationality = player.Nationality,
            TeamName = player.TeamName,
            ProfilePicturePath = player.ProfilePicturePath,
            Description = player.Description
        };

        return View(playerDetails);
    }

    // POST: Delete
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var player = await _playerService.GetPlayerByIdAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        var picturePath = Path.Combine("wwwroot/images/Players", player.ProfilePicturePath.TrimStart('/'));

        if (System.IO.File.Exists(picturePath))
        {
            System.IO.File.Delete(picturePath);
        }

        var success = await _playerService.DeletePlayerAsync(id);
        if (success)
        {
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View("Delete", player);
        }
    }
}
