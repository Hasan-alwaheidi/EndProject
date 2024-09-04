using EndProject.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace EndProject.Controllers
{
    public class LiveScoresController : Controller
    {
        private readonly LiveScoreService _liveScoreService;

        public LiveScoresController(LiveScoreService liveScoreService)
        {
            _liveScoreService = liveScoreService;
        }

        public async Task<IActionResult> Index()
        {
            JObject liveScoresData = await _liveScoreService.GetLiveScoresAsync();

            var liveScores = liveScoresData["matches"] as JArray;

            ViewBag.LiveScores = liveScores;

            return View();
        }

    }
}
