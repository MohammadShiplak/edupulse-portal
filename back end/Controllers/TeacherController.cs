using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.Services;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeacherController : ControllerBase
    {

        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
          _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDTO>>> GetAllTeachers(int pageNumber=1,int pageSize=10)
        {
            var teacher = await _teacherService.GetAllTeachersAsync(pageNumber,pageSize);
            return Ok(teacher);

        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>DeleteTeacher(int id)
        {
            var deleted = await _teacherService.DeleteTeacherAsync(id);

            if (!deleted)
                return NotFound($"Teacher with Id {id} was not found");


            return Ok($"Teacher with Id {id} deleted successfully");

        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<TeacherDTO>> GetTeachersInfobyId(int id)
        {
            if (id < 0)
                return BadRequest($"Id '{id}' is not valid. Id must be a positive number");

            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
                return NotFound($"Teacher with Id {id} was not found");

            return Ok(teacher);



        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TeacherDTO>> AddTeachers(TeacherDTO createdTeacher)
        {

            var teacher = await _teacherService.AddTeachersAsync(createdTeacher)  ;

            return CreatedAtAction(nameof(GetTeachersInfobyId), new { id = teacher.Id }, teacher);


        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TeacherDTO>>UpdateTeacher(int id,TeacherDTO updatedTeacher)
        {
            var teacher = await _teacherService.UpdateTeachersAsync(id,updatedTeacher);

            if (teacher == null)
                return NotFound($"Student with Id {id} was not found");

            return Ok(teacher);

        }


    

    }
}
