using EndProject.Services;
using FootballApiProject.Models.DTO_s.MatchsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SharedDtos.DTO_s.NewsDto;
using System.Linq;

public class HomeController : Controller
{
    private readonly IMatchService _matchService;
    private readonly ILogger<HomeController> _logger;
    private readonly INewsService _newsService;
    private readonly LiveScoreService _liveScoreService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public HomeController(IMatchService matchService, ILogger<HomeController> logger, INewsService newsService, LiveScoreService liveScoreService, IWebHostEnvironment hostingEnvironment)
    {
        _matchService = matchService;
        _logger = logger;
        _newsService = newsService;
        _liveScoreService = liveScoreService;
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Fetching matches for the home page.");
        var matches = await _matchService.GetMatchesAsync();
        if (matches == null)
        {
            _logger.LogWarning("No matches found.");
            matches = new List<MatchDto>();
        }

        _logger.LogInformation("Fetching slideshow images.");
        // Update the path to point to the slideshow folder
        var imageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "slideshow");
        var imagePaths = Directory.GetFiles(imageFolder)
                                  .Select(f => "/images/slideshow/" + Path.GetFileName(f))
                                  .ToList();

        _logger.LogInformation("Fetching news for the home page.");
        var newsItems = await _newsService.GetNewsItemsAsync();
        if (newsItems == null)
        {
            _logger.LogWarning("No news items found.");
            newsItems = new List<NewsDtoo>();
        }
        var newsDetailsItems = newsItems.Select(news => new NewsDetailsDto
        {
            Id = news.Id,
            Title = news.Title,
            Content = news.Content,
            DatePublished = news.DatePublished,
            ImagePath = news.ImagePath
        }).ToList();

        JObject liveScores = await _liveScoreService.GetLiveScoresAsync();
        JArray liveScoresArray = liveScores["data"]?["match"] as JArray ?? new JArray();
        var prioritizedCompetitions = new List<string>
    {
        "LaLiga Santander",
        "Premier League",
        "Serie A",
        "Bundesliga",
        "Ligue 1",
        "Champions League",
        "Europa League",
        "UEFA Nations League"

    };

        var sortedLiveScores = liveScoresArray
            .OrderBy(match =>
            {
                var competitionName = match["competition"]?["name"]?.ToString();
                var priority = prioritizedCompetitions.IndexOf(competitionName);
                return priority == -1 ? int.MaxValue : priority; 
            })
            .ThenByDescending(match => match["status"].ToString() == "IN PLAY")
            .ToList();

        var model = new HomeViewModel
        {
            Matches = matches.ToList(),
            SlideshowImages = imagePaths,
            NewsItems = newsItems.ToList(),
            LiveScores = new JArray(sortedLiveScores)
        };

        _logger.LogInformation("Successfully fetched matches, slideshow images, and news items.");
        return View(model);
    }

}
