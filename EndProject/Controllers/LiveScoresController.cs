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

            // Extract the relevant data from the JObject
            var liveScores = liveScoresData["matches"] as JArray;

            // Pass the data to the View using ViewBag
            ViewBag.LiveScores = liveScores;

            return View();
        }

    }
}
