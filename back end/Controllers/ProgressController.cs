// File: Controllers/ProgressController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_Project_API.Data.Config;
using School_Project_API.DTO;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        // POST /api/progress/calculate?studentId=1&courseId=2
        // WHY POST not GET for calculate?
        // It WRITES to the database (saves the result).
        // GET should be read-only. POST changes data.
        [HttpPost("calculate")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<CourseProgressDTO>> Calculate(
            int studentId, int courseId)
        {
            try
            {
                var result = await _progressService
                    .CalculateAndSaveAsync(studentId, courseId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/progress/student/1/course/2
        [HttpGet("student/{studentId}/course/{courseId}")]
        public async Task<ActionResult<CourseProgressDTO>> GetProgress(
            int studentId, int courseId)
        {
            var progress = await _progressService
                .GetProgressAsync(studentId, courseId);

            if (progress == null)
                return NotFound(
                    $"No progress calculated yet for student {studentId} in course {courseId}. " +
                    "Call POST /api/progress/calculate first.");

            return Ok(progress);
        }

        // GET /api/progress/student/1
        // Returns ALL courses progress for student 1
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<CourseProgressDTO>>> GetStudentProgress(
            int studentId)
        {
            var progressList = await _progressService
                .GetStudentProgressAsync(studentId);
            return Ok(progressList);
        }

        // GET /api/progress/course/2
        // Returns ALL students' progress in course 2
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<List<CourseProgressDTO>>> GetCourseProgress(
            int courseId)
        {
            var progressList = await _progressService
                .GetCourseProgressAsync(courseId);
            return Ok(progressList);
        }
    }
}