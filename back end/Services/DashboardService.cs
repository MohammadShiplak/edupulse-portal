
using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;

using School_Project_API.Entities;
using School_Project_API.Services.Interfaces;
using static School_Project_API.DTO.DashboardDTO;

namespace School_Project_API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDTO> GetDashboardStatsAsync()
        {
            var today = DateTime.UtcNow.Date;

            // ===========================
            // General Statistics
            // ===========================

            var totalStudents = await _context.Students
                .AsNoTracking()
                .CountAsync();

            var totalTeachers = await _context.Teacher
                .AsNoTracking()
                .CountAsync();

            var totalDepartments = await _context.Departments
                .AsNoTracking()
                .CountAsync();

            var totalCourses = await _context.Course
                .AsNoTracking()
                .CountAsync();

            var totalClasses = await _context.Class
                .AsNoTracking()
                .CountAsync();

            var totalSubjects = await _context.Subjects
                .AsNoTracking()
                .CountAsync();

            // ===========================
            // Homework Statistics
            // ===========================

            var homeworkStats = await _context.Homeworks
                .AsNoTracking()
                .GroupBy(h => 1)
                .Select(g => new
                {
                    Active = g.Count(h => h.Status == HomeworkStatus.Active),
                    Archived = g.Count(h => h.Status == HomeworkStatus.Archived)
                })
                .FirstOrDefaultAsync();

            // ===========================
            // Attendance Statistics
            // ===========================

            var attendanceStats = await _context.Attendances
                .AsNoTracking()
                .Where(a => a.Date.HasValue &&
                            a.Date.Value.Date == today)
                .GroupBy(a => 1)
                .Select(g => new
                {
                    Present = g.Count(a => a.Status == AttendanceStatus.Present),
                    Absent = g.Count(a => a.Status == AttendanceStatus.Absent),
                    Late = g.Count(a => a.Status == AttendanceStatus.Late)
                })
                .FirstOrDefaultAsync();

            // ===========================
            // Recent Homework
            // ===========================

            var recentHomework = await _context.Homeworks
                .AsNoTracking()
                .Include(h => h.Teacher)
                .Include(h => h.Class)
                .OrderByDescending(h => h.CreatedAt)
                .Take(5)
                .Select(h => new RecentHomeworkDTO
                {
                    Id = h.Id,
                    Title = h.Title,
                    TeacherName = h.Teacher.Name,
                    ClassName = h.Class.Name,
                    DueDate = h.DueDate,
                    Status = h.Status.ToString()
                })
                .ToListAsync();

            // ===========================
            // Return DTO
            // ===========================

            return new DashboardDTO
            {
                TotalStudents = totalStudents,
                TotalTeachers = totalTeachers,
                TotalDepartments = totalDepartments,
                TotalCourses = totalCourses,
                TotalClasses = totalClasses,
                TotalSubjects = totalSubjects,

                ActiveHomework = homeworkStats?.Active ?? 0,
                ArchivedHomework = homeworkStats?.Archived ?? 0,

                TodayPresent = attendanceStats?.Present ?? 0,
                TodayAbsent = attendanceStats?.Absent ?? 0,
                TodayLate = attendanceStats?.Late ?? 0,

                RecentHomework = recentHomework
            };
        }
    }
}