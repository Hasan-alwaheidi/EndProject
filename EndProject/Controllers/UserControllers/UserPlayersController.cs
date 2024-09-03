using EndProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EndProject.Controllers.UserControllers
{
    public class UserPlayersController : Controller
    {
        private readonly IPlayerService _playerService;

        public UserPlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<IActionResult> Index()
        {
            var players = await _playerService.GetPlayersAsync();
            return View(players);
        }

        public async Task<IActionResult> Details(int id)
        {
            var playerDetails = await _playerService.GetPlayerByIdAsync(id);
            if (playerDetails == null)
            {
                return NotFound();
            }
            return View(playerDetails);
        }


    }
}
