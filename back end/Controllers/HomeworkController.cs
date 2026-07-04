using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School_Project_API.DTO;
using School_Project_API.helper;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeworkController : ControllerBase
    {


        private readonly IHomeworkService _homeworkService;

   
        public HomeworkController(IHomeworkService homeworkService)
        {
            _homeworkService = homeworkService;
        }


        [HttpGet("{id:int}")]
    
        public async Task<ActionResult<HomeworkDTO>> GetHomeworkById(int id)
        {
            if (id <= 0)
                return BadRequest($"Id '{id}' is not valid. Id must be a positive number.");

            var homework = await _homeworkService.GetHomeworkByIdAsync(id);

            if (homework == null)
                return NotFound($"Homework with Id {id} was not found.");

            return Ok(homework);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<HomeworkDTO>>> GetAllHomework(
            int pageNumber = 1,
            int pageSize = 10)

        {
            var result = await _homeworkService.GetAllHomeworkAsync(pageNumber, pageSize);
            return Ok(result);
        }

      
        [HttpGet("teacher/{teacherId:int}")]
        
        public async Task<ActionResult<List<HomeworkDTO>>> GetHomeworkByTeacher(int teacherId)
        {
            var homeworks = await _homeworkService.GetHomeworkByTeacherAsync(teacherId);
            return Ok(homeworks);
        }


        [HttpGet("class/{classId:int}")]
        public async Task<ActionResult<List<HomeworkDTO>>> GetHomeworkByClass(int classId)
        {
            var homeworks = await _homeworkService.GetHomeworkByClassAsync(classId);
            return Ok(homeworks);
        }

      
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<HomeworkDTO>> AddHomework([FromForm] HomeworkCreateDTO homeworkDTO)
        {
       
            try
            {
                var created = await _homeworkService.AddHomeworkAsync(homeworkDTO);

              
                return CreatedAtAction(
                    nameof(GetHomeworkById),        
                    new { id = created.Id },         
                    created                          
                );
            }
            catch (InvalidOperationException ex)
            {
               
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        [Consumes("multipart/form-data")]
      
        public async Task<ActionResult<HomeworkDTO>> UpdateHomework(int id, [FromForm] HomeworkCreateDTO homeworkDTO)
        {
            var updated = await _homeworkService.UpdateHomeworkAsync(id, homeworkDTO);

            if (updated == null)
                return NotFound($"Homework with Id {id} was not found.");

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteHomework(int id)
        {
     
            var deleted = await _homeworkService.DeleteHomeworkAsync(id);

            if (!deleted)
                return NotFound($"Homework with Id {id} was not found.");

            return Ok($"Homework with Id {id} deleted successfully.");
        }


        [HttpDelete("{id:int}/file")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteHomeworkFile(int id)
        {
            var deleted = await _homeworkService.DeleteHomeworkFileAsync(id);

            if (!deleted)
                return NotFound($"Homework {id} not found or has no file to delete.");

            return Ok($"File for Homework {id} deleted successfully.");
        }

    }

}

