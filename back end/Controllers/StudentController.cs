using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.helper;
using School_Project_API.Services;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;








        public StudentController(IStudentService studentService)
        {
           _studentService = studentService;    
        }


       
        [HttpGet("{id}", Name = "GetStudentsInfoByID")]

        public async Task<ActionResult<StudentDTO>> GetStudentsByID(int id)
        {

            if (id < 0)
                return BadRequest($"Id '{id}' is not valid. Id must be a positive number");

                    var student =await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound($"Student with Id {id} was not found");

            return Ok(student);




        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<StudentDTO>>> GetAllStudents(int pageNumber=1,int pageSize=10)
        {
          
var students =await _studentService.GetAllStudentsAsync(pageNumber,pageSize);
            return Ok(students);

          
        }



        /*
         
        in this case 


        it is so significant to create a  DTO in order to send required information to server 

        and void unNecessary information 


        */


        [HttpPost(Name = "AddStudents")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StudentDTO>> AddStudents(StudentDTO student)
        {

            var createdStudent=await _studentService.AddStudentAsync(student);

            return CreatedAtAction(nameof(GetStudentsByID),new {id=createdStudent.Id},createdStudent);



        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StudentDTO>> UpdateStudents(int id,StudentDTO studentDTO)
        {

            var updatedstudent = await _studentService.UpdateStudentAsync(id, studentDTO);

if (updatedstudent == null)
                return NotFound($"Student with Id {id} was not found");

return Ok(updatedstudent);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(int Id)
        {
            var deleted =await _studentService.DeleteStudentAsync(Id);

            if (!deleted)
                return NotFound($"Student with Id {Id} was not found");


            return Ok($"Student with Id {Id} deleted successfully");
        }

    }
}