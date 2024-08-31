using EndProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace EndProject.Controllers.UserControllers
{
   
        public class UserStadiumsController : Controller
        {
            private readonly IStadiumService _stadiumService;

            public UserStadiumsController(IStadiumService stadiumService)
            {
                _stadiumService = stadiumService;
            }

            public async Task<IActionResult> Index()
            {
                var stadiums = await _stadiumService.GetStadiumsAsync();
                return View(stadiums);
            }

            public async Task<IActionResult> Details(int id)
            {
                var stadium = await _stadiumService.GetStadiumByIdAsync(id);
                if (stadium == null)
                {
                    return NotFound();
                }
                return View(stadium);
            }
        }
    }

