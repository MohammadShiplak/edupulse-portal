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
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AttendanceDTO>>> GetAllAttendance()
        {
            var attendance = await _attendanceService.GetAllAttendancesAsync();
            return Ok(attendance);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceDTO>> GetAttendanceById(int id)
        {
            var attendance = await _attendanceService.GetAttendanceByIdAsync(id);

            if (attendance == null)
                return NotFound($"Attendance with Id {id} was not found");

            return Ok(attendance);
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<List<AttendanceDTO>>> GetAttendanceByDate(DateTime date)
        {
            var attendance = await _attendanceService.GetAttendanceByDateAsync(date);
            return Ok(attendance);
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<AttendanceDTO>>> GetAttendanceByStudent(int studentId)
        {
            var attendance = await _attendanceService.GetAttendanceByStudentAsync(studentId);
            return Ok(attendance);
        }

        [HttpPost]
     
        public async Task<ActionResult<AttendanceDTO>> AddAttendance(AttendanceDTO attendanceDTO)
        {
            try
            {
                var createdAttendance = await _attendanceService.AddAttendanceAsync(attendanceDTO);

                if (createdAttendance == null)
                    return NotFound($"Student with Id {attendanceDTO.StudentId} was not found");

                return CreatedAtAction(
                    nameof(GetAttendanceById),
                    new { id = createdAttendance.Id },
                    createdAttendance
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
       
        public async Task<ActionResult<AttendanceDTO>> UpdateAttendance(
            int id,
            AttendanceDTO attendanceDTO)
        {
            var updatedAttendance = await _attendanceService.UpdateAttendanceAsync(id, attendanceDTO);

            if (updatedAttendance == null)
                return NotFound($"Attendance with Id {id} was not found");

            return Ok(updatedAttendance);
        }

        [HttpDelete("{id}")]
   
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var deleted = await _attendanceService.DeleteAttendanceAsync(id);

            if (!deleted)
                return NotFound($"Attendance with Id {id} was not found");

            return Ok($"Attendance with Id {id} deleted successfully");
        }
    }
}
