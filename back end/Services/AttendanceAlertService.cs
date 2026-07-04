using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Services
{
    public class AttendanceAlertService : IAttendanceAlertService
    {

        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService; 

        public AttendanceAlertService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService; 
        }

        private static AttendanceAlertDTO MapToDTO(AttendanceAlert alert)
        {
            return new AttendanceAlertDTO
            {
                Id = alert.Id,
                StudentId = alert.StudentId,
                // WHY null-conditional operator (?.):
                //   If Student is null (student was deleted), we get null
                //   instead of a NullReferenceException crash.
                StudentName = alert.Student != null
                    ? $"{alert.Student.FirstName} {alert.Student.LastName}"
                    : $"Student #{alert.StudentId}",
                ConsecutiveAbsences = alert.ConsecutiveAbsences,
                AlertDate = alert.AlertDate,
                Status = alert.Status,
                CreatedAt = alert.CreatedAt,
                Notes = alert.Notes
            };
        }



        public async Task CheckAndCreateAlertAsync(int studentId, DateTime date)
        {


            var recentAttendances = await _context.Attendances
                .Where(a => a.StudentId == studentId && a.Date.HasValue)
                .OrderByDescending(a => a.Date)
                .Take(3)
                .ToListAsync();

            if (recentAttendances.Count < 3)
                return;

            bool allAbsent=recentAttendances.All(a=>a.Status == AttendanceStatus.Absent);

            if (!allAbsent)
                return;

            var existingAlert = await _context.AttendanceAlerts
                .AnyAsync(a => a.StudentId == studentId && a.Status == AlertStatus.Active);
                
                if(existingAlert)
                return;

                var student = await _context.Students.FirstOrDefaultAsync(s=>s.Id == studentId);

            var studentName = student != null ? $"{student.FirstName} {student.LastName}" : $"Student #{studentId}";



            var newAlert = new AttendanceAlert
            {
                StudentId = studentId,
                ConsecutiveAbsences = 3,
                AlertDate = date,
                Status = AlertStatus.Active
            };  

            _context.AttendanceAlerts.Add(newAlert);
            await _context.SaveChangesAsync();

            await _notificationService.SendToRoleAsync(
               role: "Admin",
               message: $"🚨 ALERT: {studentName} has been absent for 3 consecutive days! " +
                        $"Last absence: {date:MMM dd, yyyy}. Please contact the parent.",
               type: "warning"
           );

            // WHY also send to Teacher role:
            //   Teachers should be aware too, even if they don't take action.
            await _notificationService.SendToRoleAsync(
                role: "Teacher",
                message: $"⚠️ {studentName} has 3 consecutive absences as of {date:MMM dd, yyyy}.",
                type: "warning"
            );


        
        }

        public async  Task<int> GetActiveAlertCountAsync()
        {
            return await _context.AttendanceAlerts
                  .CountAsync(a => a.Status == AlertStatus.Active);
        }

        public async  Task<List<AttendanceAlertDTO>> GetActiveAlertsAsync()
        {
            var alerts = await _context.AttendanceAlerts
                         .Include(a => a.Student)
                         .OrderByDescending(a => a.AlertDate) // newest alerts first
                         .ToListAsync();

            // Map to DTOs in C# (NOT inside .Select() for EF Core)
            // WHY map after ToListAsync():
            //   MapToDTO calls Path.GetFileName and string interpolation.
            //   These are C# methods — EF Core can't translate them to SQL.
            //   So we fetch first, then transform.
            //   This is the EXACT lesson from your HomeworkService fix!
            return alerts.Select(MapToDTO).ToList();
        }

        public async  Task<List<AttendanceAlertDTO>> GetAlertsByStudentAsync(int studentId)
        {
            var alerts = await _context.AttendanceAlerts
                .Include(a => a.Student)
                .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.AlertDate)
                .ToListAsync();

            return alerts.Select(MapToDTO).ToList();
        }

        public async  Task<List<AttendanceAlertDTO>> GetAllAlertsAsync()
        {
            var alerts = await _context.AttendanceAlerts
                  .Include(a => a.Student)
                  .OrderByDescending(a => a.AlertDate) // newest alerts first
                  .ToListAsync();

            // Map to DTOs in C# (NOT inside .Select() for EF Core)
            // WHY map after ToListAsync():
            //   MapToDTO calls Path.GetFileName and string interpolation.
            //   These are C# methods — EF Core can't translate them to SQL.
            //   So we fetch first, then transform.
            //   This is the EXACT lesson from your HomeworkService fix!
            return alerts.Select(MapToDTO).ToList();
        }

        public async  Task<AttendanceAlertDTO?> ResolveAlertAsync(int alertId, ResolveAlertDTO resolveDTO)
        {
            // WHY Include here: we need Student for the response DTO
            // WHY FirstOrDefaultAsync (not FindAsync): FindAsync doesn't support Include
            var alert = await _context.AttendanceAlerts
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == alertId);

            if (alert == null)
                return null;

            // WHY only update Status and Notes:
            //   StudentId, AlertDate, ConsecutiveAbsences, CreatedAt are
            //   HISTORICAL FACTS. They should never change after creation.
            //   Only the resolution status can change.
            alert.Status = resolveDTO.Status;
            alert.Notes = resolveDTO.Notes;

            // EF Core tracks this entity (we loaded it with .FirstOrDefaultAsync)
            // .SaveChangesAsync() detects the changed properties and runs UPDATE
            await _context.SaveChangesAsync();

            return MapToDTO(alert);
        }
    }
}
