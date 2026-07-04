using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Services.Interfaces;
using static School_Project_API.DTO.ClassSubjectDTO;

namespace School_Project_API.Entities
{
    public class ClassSubjectService : IClassSubjectService
    {

        private readonly ApplicationDbContext _context;

        private readonly INotificationService _notificationService;


        public ClassSubjectService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context; 
            _notificationService = notificationService; 
        }

        private static ClassSubjectReadDTO MapToReadDTO(ClassSubject cs)
        {
            return new ClassSubjectReadDTO
            {
                ClassId = cs.ClassId,
                SubjectId = cs.SubjectId,

                // WHY null-coalescing ("??") and ("Unknown") fallback?
                //   Navigation properties (cs.Class, cs.Subject) might not be loaded
                //   if we forgot to .Include() them in the query.
                //   The fallback prevents NullReferenceException.
                //   If you see "Unknown" in the UI, it means you forgot an Include().
                ClassName = cs.Class?.Name ?? "Unknown",
                SubjectName = cs.Subject?.SubjectName ?? "Unknown",

                AssignedAt = cs.AssignedAt
            };
        }












        public async Task<ClassSubjectDTO.ClassSubjectReadDTO> AssignSubjectToClassAsync(ClassSubjectDTO.ClassSubjectWriteDTO dto)
        {
        var classExists=await _context.Class
                .AnyAsync(c=>c.Id == dto.ClassId);  

            if(!classExists)
                throw new InvalidOperationException(
                   $"Class with Id {dto.ClassId} does not exist.");

            var subjectExists=await _context.Subjects
                .AnyAsync(s=>s.Id ==  dto.SubjectId);

            if (!subjectExists)
                throw new InvalidOperationException(
                    $"Subject with Id {dto.SubjectId} does not exist.");

            var alreadyAssigned = await IsSubjectAssignedToClassAsync(dto.ClassId, dto.SubjectId);

            if (alreadyAssigned)
                throw new InvalidOperationException(
                    $"Subject with Id {dto.SubjectId} is already assigned to Class with Id {dto.ClassId}.");

            var classSubject = new ClassSubject
            {
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId
                // AssignedAt is set by the C# default property initializer
            };

            _context.ClassSubjects.Add(classSubject);

            // ── SAVE TO DATABASE ──────────────────────────────────────
            // WHY SaveChangesAsync() and not SaveChanges()?
            //   Async = the thread is FREE while SQL Server processes the INSERT.
            //   The server can handle other requests during this time.
            //   Sync version (.SaveChanges()) would BLOCK the thread.
            //   Always use async in web APIs.
            //
            // REMINDER FROM YOUR PROJECT HISTORY:
            //   Never forget SaveChangesAsync()! Missing it = silent failure.
            //   The entity gets added to EF Core's tracking but never hits the DB.
            await _context.SaveChangesAsync();

            // ── LOAD NAVIGATION PROPERTIES ────────────────────────────
            // After saving, classSubject.Class and classSubject.Subject are NULL
            // (EF didn't load them — we only set the FK integer IDs).
            // We need them for MapToReadDTO to return names.
            //
            // WHY Entry().Reference().LoadAsync() pattern?
            //   This is the same pattern used in AttendanceService.cs in your project.
            //   It loads ONE navigation property (a single related entity, not a collection).
            //   More efficient than re-querying the whole record with Include().
            await _context.Entry(classSubject).Reference(cs => cs.Class).LoadAsync();
            await _context.Entry(classSubject).Reference(cs => cs.Subject).LoadAsync();

            // ── SEND SIGNALR NOTIFICATION ─────────────────────────────
            // Notify Admins and Teachers in real-time that a new subject was assigned.
            // WHY? When an admin assigns a subject while a teacher is logged in,
            //   the teacher sees the notification immediately without refreshing.
            var className = classSubject.Class?.Name ?? $"Class #{dto.ClassId}";
            var subjectName = classSubject.Subject?.SubjectName ?? $"Subject #{dto.SubjectId}";

            await _notificationService.SendToRoleAsync(
                role: "Admin",
                message: $"📚 Subject '{subjectName}' was assigned to {className}",
                type: "info"
            );

            await _notificationService.SendToRoleAsync(
                role: "Teacher",
                message: $"📚 Subject '{subjectName}' added to {className}",
                type: "info"
            );

            // ── MAP AND RETURN ────────────────────────────────────────
            return MapToReadDTO(classSubject);





        }

        public async Task<List<ClassSubjectDTO.ClassSubjectReadDTO>> GetClassesBySubjectAsync(int subjectId)
        {

            var result = await _context.ClassSubjects
                .AsNoTracking()
                .Where(c => c.SubjectId == subjectId)
                .Select(c => new ClassSubjectReadDTO
                {

                    ClassId = c.ClassId,
                    SubjectId = c.SubjectId,
                    ClassName = c.Class.Name,
                    SubjectName = c.Subject.SubjectName,
                    AssignedAt = c.AssignedAt
                })
                .OrderBy(s =>s.ClassName)
                .ToListAsync();


            return result;
        }

        public async Task<List<ClassSubjectDTO.ClassSubjectReadDTO>> GetSubjectsByClassAsync(int classId)
        {


       
            var result = await  _context.ClassSubjects
                .AsNoTracking()
                .Where(c => c.ClassId == classId)
                .Select(c => new ClassSubjectReadDTO
                {

                    ClassId = c.ClassId,
                    SubjectId = c.SubjectId,
                    ClassName = c.Class.Name,
                    SubjectName = c.Subject.SubjectName,
                    AssignedAt = c.AssignedAt
                })
                .OrderBy(s => s.SubjectName)
                .ToListAsync();


            return result;



        }

        public Task<bool> IsSubjectAssignedToClassAsync(int classId, int subjectId)
        {
            return _context.ClassSubjects
                 .AnyAsync(sc => sc.ClassId == classId && sc.SubjectId == subjectId);    
        }

        public async Task<bool> RemoveSubjectFromClassAsync(int classId, int subjectId)
        {
            // WHY FirstOrDefaultAsync here (not AnyAsync)?
            //   AnyAsync only tells us IF it exists.
            //   We need the ACTUAL ENTITY to call _context.Remove() on it.
            //   You can't Remove() a boolean.
            //
            // WHY NOT FindAsync(classId, subjectId)?
            //   FindAsync works for simple PKs (a single int Id).
            //   For composite PKs (ClassId + SubjectId), FindAsync also works
            //   but the argument order matters and can be confusing.
            //   FirstOrDefaultAsync with a .Where() condition is more readable.
            var classSubject = await _context.ClassSubjects
                .FirstOrDefaultAsync(cs => cs.ClassId == classId && cs.SubjectId == subjectId);

            // If not found, return false → controller returns 404
            if (classSubject == null)
                return false;

            _context.ClassSubjects.Remove(classSubject);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
