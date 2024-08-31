using EndProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace EndProject.Controllers.UserControllers
{
    public class UserTeamsController : Controller
    {
        private readonly ITeamService _teamService;

        public UserTeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await _teamService.GetTeamsAsync();
            return View(teams);
        }

        public async Task<IActionResult> Details(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }
    }
}
