using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School_Project_API.Services.Interfaces;
using static School_Project_API.DTO.ClassSubjectDTO;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassSubjectController : ControllerBase
    {
        private readonly IClassSubjectService _classSubjectService;

        // WHY IClassSubjectService (interface) not ClassSubjectService (class)?
        //   Program.cs registers: services.AddScoped<IClassSubjectService, ClassSubjectService>()
        //   ASP.NET Core injects the registered implementation at runtime.
        //   If you ever swap ClassSubjectService for a different implementation,
        //   you change ONE line in Program.cs — not every controller.
        public ClassSubjectController(IClassSubjectService classSubjectService)
        {
            _classSubjectService = classSubjectService;
        }

        // ════════════════════════════════════════════════════════════════
        // GET /api/ClassSubject/class/1
        // "Give me all subjects assigned to Class 1"
        // ════════════════════════════════════════════════════════════════
        [HttpGet("class/{classId:int}")]
        // WHY :int? Route constraint.
        //   /class/abc → 404 (not matched, "abc" is not an int)
        //   /class/1   → ✅ matched, classId = 1
        //   Without it: /class/abc would match and you'd get a binding error later.
        public async Task<ActionResult<List<ClassSubjectReadDTO>>> GetSubjectsByClass(int classId)
        {
            if (classId <= 0)
                return BadRequest($"Class Id '{classId}' is not valid. Must be a positive number.");

            var subjects = await _classSubjectService.GetSubjectsByClassAsync(classId);

            // WHY return Ok() even when the list is empty?
            //   An empty list is a VALID response — the class exists but has no subjects.
            //   404 would mean "this endpoint doesn't exist", which is wrong.
            //   [] (empty array) = "I found the class, but it has no subjects assigned."
            return Ok(subjects);
        }

        // ════════════════════════════════════════════════════════════════
        // GET /api/ClassSubject/subject/1
        // "Give me all classes that teach Subject 1"
        // ════════════════════════════════════════════════════════════════
        [HttpGet("subject/{subjectId:int}")]
        public async Task<ActionResult<List<ClassSubjectReadDTO>>> GetClassesBySubject(int subjectId)
        {
            if (subjectId <= 0)
                return BadRequest($"Subject Id '{subjectId}' is not valid. Must be a positive number.");

            var classes = await _classSubjectService.GetClassesBySubjectAsync(subjectId);
            return Ok(classes);
        }

        // ════════════════════════════════════════════════════════════════
        // POST /api/ClassSubject
        // Body: { "classId": 1, "subjectId": 2 }
        // "Assign Subject 2 to Class 1"
        // ════════════════════════════════════════════════════════════════
        [HttpPost]
        [Authorize(Roles = "Admin")]
        // WHY Admin only?
        //   Assigning subjects to classes is a CURRICULUM DECISION.
        //   Only admins should be able to change the curriculum.
        //   Teachers can VIEW their subjects but shouldn't reassign them.
        public async Task<ActionResult<ClassSubjectReadDTO>> AssignSubjectToClass(
            ClassSubjectWriteDTO dto)
        // WHY no [FromBody] here?
        //   [ApiController] attribute infers [FromBody] for complex types automatically.
        //   You don't need to write it explicitly (though it doesn't hurt if you do).
        {
            try
            {
                var created = await _classSubjectService.AssignSubjectToClassAsync(dto);

                // WHY CreatedAtAction instead of Ok()?
                //   REST convention: a successful POST should return 201 Created.
                //   CreatedAtAction also sets the Location header:
                //   Location: /api/ClassSubject/class/1
                //   This tells the client WHERE to find the created resource.
                //   Ok() returns 200, which means "success" but not "created".
                return CreatedAtAction(
                    nameof(GetSubjectsByClass),          // ← action to build the URL from
                    new { classId = created.ClassId },   // ← route values for that action
                    created                              // ← the response body
                );
            }
            catch (InvalidOperationException ex)
            {
                // WHY catch InvalidOperationException specifically?
                //   Our service throws this for known business errors:
                //   - Class not found
                //   - Subject not found
                //   - Already assigned
                //   These are CLIENT errors (400 Bad Request), not server errors (500).
                //   We catch them and return 400 with the clear message.
                //   ANY other exception (network, SQL crash) bubbles up to 500.
                return BadRequest(ex.Message);
            }
        }

        // ════════════════════════════════════════════════════════════════
        // DELETE /api/ClassSubject/1/2
        // "Remove Subject 2 from Class 1"
        // ════════════════════════════════════════════════════════════════
        [HttpDelete("{classId:int}/{subjectId:int}")]
        [Authorize(Roles = "Admin")]
        // WHY two route parameters in the URL (not a body)?
        //   HTTP DELETE conventionally identifies the resource in the URL.
        //   The "resource" here is the assignment between ClassId and SubjectId.
        //   So the URL IS the identifier: /api/ClassSubject/1/2 = "the assignment of Subject 2 to Class 1"
        //
        //   ALTERNATIVE: DELETE /api/ClassSubject with a body { classId, subjectId }
      //   Some APIs do this, but it's less RESTful. URLs in DELETE identify what to delete.

        public async Task<IActionResult> RemoveSubjectFromClass(int classId, int subjectId)
        {
            var deleted = await _classSubjectService.RemoveSubjectFromClassAsync(classId, subjectId);

            if (!deleted)
                return NotFound(
                    $"Assignment of Subject {subjectId} to Class {classId} was not found.");

            // WHY Ok() with a message instead of NoContent() (204)?
            //   NoContent() returns 204 with NO body.
            //   For beginners / simple frontends, Ok() with a message is clearer.
            //   Both are acceptable REST conventions for DELETE.
            //   NoContent() is the "purist" REST choice. Ok() is more user-friendly.
            return Ok($"Subject {subjectId} successfully removed from Class {classId}.");
        }



    }
}
