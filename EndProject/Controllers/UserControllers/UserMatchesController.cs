using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApiProject.Models.DTO_s.MatchsDto;
using Microsoft.AspNetCore.Mvc;
using EndProject.Services;
using EndProject.Models;

namespace EndProject.Controllers
{
    public class UserMatchesController : Controller
    {
        private readonly IMatchService _matchService;

        public UserMatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        public async Task<IActionResult> Index()
        {
            var matches = await _matchService.GetMatchesAsync();

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var oldMatches = matches.Where(m => m.Date.Date < today).OrderByDescending(m => m.Date).ToList();
            var todayMatches = matches.Where(m => m.Date.Date == today).ToList();
            var tomorrowMatches = matches.Where(m => m.Date.Date == tomorrow).ToList();
            var upcomingMatches = matches.Where(m => m.Date.Date > tomorrow).OrderBy(m => m.Date).ToList();

            var model = new MatchesViewModel
            {
                OldMatches = oldMatches,
                TodayMatches = todayMatches,
                TomorrowMatches = tomorrowMatches,
                UpcomingMatches = upcomingMatches
            };

            return View(model);
        }
    }
}
