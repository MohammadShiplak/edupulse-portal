using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

      private readonly INotificationService _notificationService;
        private readonly IAttendanceAlertService _alertService;
        public AttendanceService(ApplicationDbContext context, INotificationService notificationService, IAttendanceAlertService alertService)
        {
            _context = context;
            _notificationService = notificationService;
            _alertService = alertService;
        }

        private static AttendanceDTO MapToDTO(Attendance attendance)
        {

            return new AttendanceDTO
            {
                Id = attendance.Id,
                StudentId = attendance.StudentId,
                StudentName = attendance.Student == null ? null : $"{attendance.Student.FirstName} {attendance.Student.LastName}",
                Date = attendance.Date,
                Status = attendance.Status,
                Notes = attendance.Notes






            }; 






        }

      
            public async Task<AttendanceDTO?> AddAttendanceAsync(AttendanceDTO attendanceDTO)
            {
                // ── Step 1: Does student exist? ───────────────────────────────
                var studentExists = await _context.Students
                    .AnyAsync(s => s.Id == attendanceDTO.StudentId);

                if (!studentExists)
                    return null;

                // ── Step 2: Already recorded today? ──────────────────────────
                var alreadyExists = await _context.Attendances
                    .AnyAsync(a => a.StudentId == attendanceDTO.StudentId &&
                              a.Date.HasValue &&
                              a.Date.Value.Date == attendanceDTO.Date.Value.Date);
                // ↑ compares only the date part, ignores time

                if (alreadyExists)
                    throw new InvalidOperationException(
                        "Attendance already exists for this student");

                // ── Step 3: Create entity ─────────────────────────────────────
                var attendance = new Attendance
                {
                    StudentId = attendanceDTO.StudentId,
                    Date = attendanceDTO.Date,
                    Status = attendanceDTO.Status,
                    Notes = attendanceDTO.Notes
                };

                // ── Step 4: Save → DB assigns real Id ────────────────────────
                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();

                // ── Step 5: Load student name for response ────────────────────
                await _context.Entry(attendance)
                     .Reference(a => a.Student)
                     .LoadAsync();

            var statusLabel = attendance.Status.ToString();

            var studentName=attendance.Student != null ? $"{attendance.Student.FirstName} {attendance.Student.LastName}" : $"Student #{attendance.StudentId}";

            var dateLabel = attendance.Date?.ToString("MMM dd, yyyy") ?? "today";

            await _notificationService.SendToRoleAsync(
                role: "Admin",
                message: $"📋 {studentName} marked {statusLabel} on {dateLabel}", // ← use dateLabel
                type: attendance.Status == AttendanceStatus.Absent ? "warning" : "success"    
            );



            // ── Step 7: NEW — Check for consecutive absence alert ──────────
            // WHY only check when status is Absent:
            //   If student is Present/Late/Excused, there's no consecutive
            //   absence streak to check. Skip the check entirely — faster.
            //
            // WHY after SaveChangesAsync (not before):
            //   CheckAndCreateAlertAsync queries the DB for recent attendances.
            //   The record we just saved MUST be in the DB before we query it.
            //   If we check BEFORE saving, the current absence isn't in the DB yet.
            if (attendance.Status == AttendanceStatus.Absent)
            {
                await _alertService.CheckAndCreateAlertAsync(
                    attendance.StudentId!.Value,
                    attendance.Date!.Value
                );
            }



            // ── Step 6: Return DTO with real Id ──────────────────────────
            return MapToDTO(attendance);
            }



        

        public async  Task<bool> DeleteAttendanceAsync(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);

            if (attendance == null)
                return false;

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<AttendanceDTO>> GetAllAttendancesAsync()
        {
            return  await _context.Attendances.Include(a=>a.Student).Select(a => new AttendanceDTO
            {

                Id = a.Id,
                StudentId = a.StudentId,
                StudentName = a.Student.FirstName + " " + a.Student.LastName,
                Date = a.Date,
                Status = a.Status,
                Notes = a.Notes,




            })
                
             .OrderByDescending(a => a.Date)

             .ToListAsync();

          




        }

        public async  Task<List<AttendanceDTO>> GetAttendanceByDateAsync(DateTime date)
        {
            return await _context.Attendances
                     .Include(a => a.Student)
                     .Where(a => a.Date.Value.Date == date.Date)
                     .OrderBy(a => a.StudentId)
                     .Select(a => MapToDTO(a))
                     .ToListAsync();
        }

        public async Task<AttendanceDTO?> GetAttendanceByIdAsync(int id)
        {
            var attendance =await _context.Attendances
                .Include(a=> a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
                return null;

            return MapToDTO(attendance);
        }

        public async  Task<List<AttendanceDTO>> GetAttendanceByStudentAsync(int Studentid)
        {
            return await _context.Attendances
                   .Include(a => a.Student)
                   .Where(a => a.StudentId == Studentid)
                   .OrderByDescending(a => a.Date)
                   .Select(a => MapToDTO(a))
                   .ToListAsync();
        }

        public async  Task<AttendanceDTO?> UpdateAttendanceAsync(int id, AttendanceDTO attendanceDTO)
        {
            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
                return null;

            attendance.Date = attendanceDTO.Date;
            attendance.Status = attendanceDTO.Status;
            attendance.Notes = attendanceDTO.Notes;

            await _context.SaveChangesAsync();

            return MapToDTO(attendance);
        }
    }
}
