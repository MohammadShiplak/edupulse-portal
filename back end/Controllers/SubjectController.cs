using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.helper;
using School_Project_API.Services;
using School_Project_API.Services.Interfaces;
using System.Text.Json.Serialization;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectController : ControllerBase
    {

        private readonly ISubjectService _subjectService;


        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }


        [HttpGet("{id}",Name ="GetSubjectsInfoByID")]

        public async Task<ActionResult<Subject>> GetSubjectsInfobyID(int id)
        {
            if (id < 0)
                return BadRequest($"Id '{id}' is not valid. Id must be a positive number");

            var subject  = await _subjectService.GetSubjectByIdAsync(id);
            if (subject == null)
                return NotFound($"subject with Id {id} was not found");

            return Ok(subject);


        }

        [HttpPost(Name ="AddSubjects")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SubjectDTO>>AddSubjects(SubjectDTO NewSubject)
        {
            var teacher = await _subjectService.AddSubjectAsync(NewSubject);

            return CreatedAtAction(nameof(GetSubjectsInfobyID), new { id = teacher.Id }, teacher);
        }

        [HttpDelete("{id}",Name ="DeleteSubject")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<SubjectDTO>> DeleteSubject(int id)
        {

            var deleted = await _subjectService.DeleteSubjectAsync(id);

            if (!deleted)
                return NotFound($"subject with Id {id} was not found");


            return Ok($"subject with Id {id} deleted successfully");
        }

        [HttpGet]

        public async Task<ActionResult<PagedResponse<SubjectDTO>>> GetAllSubjects(int pageNumber = 1, int pageSize = 10)
        {

            var students = await _subjectService.GetAllSubjectAsync(pageNumber,pageSize); 
            return Ok(students);


        }



















    }
}
