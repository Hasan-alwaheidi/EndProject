using EndProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace EndProject.Controllers.UserControllers
{
    public class UserLeaguesController : Controller
    {
        private readonly ILeagueService _leagueService;

        public UserLeaguesController(ILeagueService leagueService)
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
            if (league == null)
            {
                return NotFound();
            }
            return View(league);
        }
    }
}
