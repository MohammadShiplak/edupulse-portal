// ClassEnrollmentController.cs
// ─────────────────────────────────────────────────────────────────
// WHY a dedicated controller:
//   Enrollment is a separate RESOURCE from Class or Student.
//   The URL structure /api/ClassEnrollment makes the API readable:
//   "Do something with enrollments."
//   Mixing enrollment endpoints into ClassController would make
//   that controller do too many things (violates SRP).
// ─────────────────────────────────────────────────────────────────

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_Project_API.DTO;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]    // All endpoints require login
    public class ClassEnrollmentController : ControllerBase
    {
        private readonly IClassEnrollmentService _enrollmentService;

        public ClassEnrollmentController(IClassEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // ── POST /api/ClassEnrollment ─────────────────────────────
        // WHY POST for enrollment (not PUT):
        //   POST = "create a new resource" (a new enrollment record).
        //   PUT = "replace an existing resource".
        //   Enrolling a student creates a NEW row in StudentClasses → POST.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ClassEnrollmentDTO>> EnrollStudent(
            EnrollStudentDTO dto)
        {
            try
            {
                var result = await _enrollmentService.EnrollStudentAsync(dto);
                // WHY 201 Created (not 200 OK):
                //   HTTP convention: POST that creates a resource returns 201.
                //   We don't have a GetById for enrollments, so we use Ok()
                //   wrapped in CreatedAtAction pointing to GetStudentsByClass.
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // WHY catch InvalidOperationException specifically:
                //   The service throws this for ALL business rule violations:
                //   - Student not found
                //   - Class not found
                //   - Already enrolled
                //   - Class full (capacity exceeded)
                //   The message is human-readable and safe to return to client.
                return BadRequest(new { message = ex.Message });
            }
        }

        // ── DELETE /api/ClassEnrollment/{studentId}/{classId} ────
        // WHY two route params (not a body):
        //   DELETE identifies the resource to delete via the URL.
        //   The "resource" here IS the combination (studentId, classId).
        //   Putting them in the URL is RESTful and standard.
        [HttpDelete("{studentId:int}/{classId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnenrollStudent(
            int studentId,
            int classId)
        {
            var deleted = await _enrollmentService.UnenrollStudentAsync(studentId, classId);

            if (!deleted)
                return NotFound(new
                {
                    message = $"Enrollment for Student {studentId} in Class {classId} was not found."
                });

            return Ok(new
            {
                message = $"Student {studentId} has been removed from Class {classId}."
            });
        }

        // ── GET /api/ClassEnrollment/class/{classId} ─────────────
        // WHY "class/{classId}" in the route:
        //   Differentiates "get by class" from "get by student".
        //   The URL reads naturally: "enrollment/class/3" = "enrollments for class 3"
        [HttpGet("class/{classId:int}")]
        public async Task<ActionResult<List<ClassEnrollmentDTO>>> GetStudentsByClass(
            int classId)
        {
            var students = await _enrollmentService.GetStudentByClassAsync(classId);
            return Ok(students);
        }

        // ── GET /api/ClassEnrollment/student/{studentId} ─────────
        [HttpGet("student/{studentId:int}")]
        public async Task<ActionResult<List<ClassEnrollmentDTO>>> GetClassesByStudent(
            int studentId)
        {
            var classes = await _enrollmentService.GetClassesByStudentAsync(studentId);
            return Ok(classes);
        }
    }
}