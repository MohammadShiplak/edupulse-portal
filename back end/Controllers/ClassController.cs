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
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDTO>>> GetAllClasses(int pageNumber = 1, int pageSize = 10)
        {

            var classes = await _classService.GetAllClassesAsync(pageNumber,pageSize);
            return Ok(classes);


        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ClassDTO>> DeleteClass(int id)
        {
            var deleted = await _classService.DeleteClassAsync(id);

            if (!deleted)
                return NotFound($"accessCard with Id {id} was not found");


            return Ok($"accessCard with Id {id} deleted successfully");
        }

        [HttpGet("{id}", Name = "GetClassesInfoByID")]

        public async Task<ActionResult<Class>> GetClassInfoByID(int id)
        {

            var c = await _classService.GetClassByIdAsync(id);

            if (c == null)
                return NotFound($"Class with Id {id} not found");



            return Ok(c);



        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<ClassDTO>> AddClass(ClassDTO newClass)
        {

            var Instertedclass = await _classService.AddClassesAsync(newClass);

            // Return the created department
            return CreatedAtAction(nameof(GetClassInfoByID), new { Id = newClass.Id }, Instertedclass);
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ClassDTO>> UpdateClass(int id, ClassDTO updatedclass)

        {
            var updateClass = await _classService.UpdateClassesAsync(id,updatedclass);

            if (updatedclass == null)
                return NotFound($"Student with Id {id} was not found");

            return Ok(updatedclass);



        }


    }
}






