using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Project_API.Data.Config;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.Services;
using School_Project_API.Services.Interfaces;
using System.Runtime.InteropServices;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="Teacher,Admin")]
    public class DepartmentController : ControllerBase
    {
     
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
         _departmentService = departmentService;    
        }


        [HttpGet("{Id}", Name = "GetDepratmentsInfoByID")]

        public async Task<ActionResult<DepartmentDTO>> GetDepratmentsInfoByID(int? Id)
        {


            if (Id < 0)
                return BadRequest($"Departments with {Id} is not Valid ");

            var Department = await _departmentService.GetDepartmentByIdAsync(Id);


            if (Department == null)
                return NotFound($"Deprtmnets with {Id} is not  Found");


            return Ok(Department);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAllDepartments()
        {

            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);


        }




        [HttpPost]

        public async Task<ActionResult<DepartmentDTO>> AddDepartments(DepartmentDTO newDepartment)
        {

           var department=await _departmentService.AddDepartmentAsync(newDepartment);

            // Return the created department
            return CreatedAtAction(nameof(GetDepratmentsInfoByID), new { Id = department.Id }, department);

        }

        [HttpDelete("{Id}")]

        public async Task<ActionResult<DepartmentDTO>> DeleteDepratment(int Id)
        {


            var deleted = await _departmentService.DeleteDepartemntAsync(Id); ;

            if (!deleted)
                return NotFound($"department with Id {Id} was not found");


            return Ok($"department with Id {Id} deleted successfully");


        }


        [HttpPut]
        public async Task<ActionResult<DepartmentDTO>> UpdateDepratment(int id, DepartmentDTO NewDepartment)
        {
            var updatedstudent = await _departmentService.UpdateDepartmentAsync(id, NewDepartment);

            if (updatedstudent == null)
                return NotFound($"Student with Id {id} was not found");

            return Ok(updatedstudent);



        }

        [HttpGet("{id:int}/statistics")]

        public async Task<ActionResult<DepartmentStatisticsDTO>>GetDepartmentStatistics(int id)
        {

            if (id <= 0)
                return BadRequest($"Id `{id}` must be a positive number.");

            var stats = await _departmentService.GetDepartmentStatisticsAsync(id);

            if (stats == null)
                return NotFound($"Department with Id {id} was not found");


            return Ok(stats);   

        }





    }
}
