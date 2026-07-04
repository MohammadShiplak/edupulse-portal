using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School_Project_API.DTO;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly IDashboardService _dashboardService;

        // WHY IDashboardService (interface), not DashboardService (class)?
        //   ASP.NET Core's DI container will inject the registered implementation.
        //   The controller doesn't care WHICH implementation it gets.
        //   This is the "Dependency Inversion Principle" — depend on abstractions.
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET /api/Dashboard/stats
        // Returns: DashboardStatsDTO (all counts + recent homework)
        //
        // WHY /stats in the route?
        //   In the future you might add:
        //   GET /api/Dashboard/stats         ← overall stats (this endpoint)
        //   GET /api/Dashboard/weekly-report ← a weekly report
        //   GET /api/Dashboard/charts        ← chart data
        //   Naming the route "stats" makes the API self-documenting.
        [HttpGet("stats")]
        public async Task<ActionResult<DashboardDTO>> GetDashboardStats()
        {
            // Controller is intentionally simple — just delegate to the service.
            // WHY no try/catch here?
            //   CountAsync() and basic reads don't throw business exceptions.
            //   If the DB is down, ASP.NET Core's middleware handles the 500 error.
            //   You'd add try/catch only for EXPECTED business errors
            //   (like "duplicate entry") where you want to return a specific HTTP code.
            var stats = await _dashboardService.GetDashboardStatsAsync();
            return Ok(stats);
        }


















    }
}
