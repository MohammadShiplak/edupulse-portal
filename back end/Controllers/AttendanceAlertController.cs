using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School_Project_API.DTO;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceAlertController : ControllerBase
    {
        private readonly IAttendanceAlertService _alertService;

        // WHY IAttendanceAlertService (not the concrete class):
        //   Same pattern as all your controllers. Depend on the interface.
        //   Program.cs maps the interface → concrete class via DI.
        public AttendanceAlertController(IAttendanceAlertService alertService)
        {
            _alertService = alertService;
        }

        // ── GET /api/AttendanceAlert ────────────────────────────────────
        // All alerts — the full report page.
        // Accessible by Admin and Teacher (teachers should see their students' alerts).
        [HttpGet]
        public async Task<ActionResult<List<AttendanceAlertDTO>>> GetAllAlerts()
        {
            var alerts = await _alertService.GetAllAlertsAsync();
            return Ok(alerts);
        }

        // ── GET /api/AttendanceAlert/active ─────────────────────────────
        // Only unresolved alerts. Used for the dashboard summary section.
        [HttpGet("active")]
        public async Task<ActionResult<List<AttendanceAlertDTO>>> GetActiveAlerts()
        {
            var alerts = await _alertService.GetActiveAlertsAsync();
            return Ok(alerts);
        }

        // ── GET /api/AttendanceAlert/count ──────────────────────────────
        // Just the count number — for the sidebar badge.
        // WHY a separate endpoint for count:
        //   Returns a single integer (e.g., 5).
        //   Way more efficient than loading all alerts just to count them.
        //   The Navbar/Sidebar can poll this cheaply.
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetActiveAlertCount()
        {
            var count = await _alertService.GetActiveAlertCountAsync();
            return Ok(count);
        }

        // ── GET /api/AttendanceAlert/student/5 ──────────────────────────
        // All alerts for a specific student.
        // Useful when viewing a student's profile page.
        [HttpGet("student/{studentId:int}")]
        public async Task<ActionResult<List<AttendanceAlertDTO>>> GetAlertsByStudent(int studentId)
        {
            if (studentId <= 0)
                return BadRequest("Student ID must be a positive number.");

            var alerts = await _alertService.GetAlertsByStudentAsync(studentId);
            return Ok(alerts);
        }

        // ── PUT /api/AttendanceAlert/3/resolve ──────────────────────────
        // Admin resolves or dismisses an alert.
        // WHY PUT (not PATCH or POST):
        //   PUT = update an existing resource.
        //   PATCH = partial update. PUT is simpler and common for status changes.
        //   POST = create a new resource. Wrong for resolving.
        //
        // WHY [Authorize(Roles = "Admin")]:
        //   Only admins should be able to resolve alerts.
        //   Teachers can SEE alerts but not dismiss them.
        [HttpPut("{id:int}/resolve")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<AttendanceAlertDTO>> ResolveAlert(
            int id,
            [FromBody] ResolveAlertDTO resolveDTO)
        {
            if (id <= 0)
                return BadRequest("Alert ID must be a positive number.");

            var updated = await _alertService.ResolveAlertAsync(id, resolveDTO);

            if (updated == null)
                return NotFound($"Alert with Id {id} was not found.");

            return Ok(updated);
        }
    }
}
