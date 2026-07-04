using Microsoft.EntityFrameworkCore;
using School_Project_API.Data.Config;
using School_Project_API.DTO;
using School_Project_API.Entities;

namespace School_Project_API.Services
{
    public class ProgressService : IProgressService
    {

        private readonly ApplicationDbContext _context; 



        private const decimal HomeworkWeight = 0.40m;
        private const decimal AttendanceWeight = 0.30m;
        private const decimal ExamWeight = 0.30m;


        private static decimal ConvertLetterGrade(string? grade)
        {
            return grade?.Trim().ToUpper() switch
            {
                "A+" => 100m,
                "A" => 95m,
                "A-" => 90m,
                "B+" => 87m,
                "B" => 83m,
                "B-" => 80m,
                "C+" => 77m,
                "C" => 73m,
                "C-" => 70m,
                "D" => 65m,
                "F" => 0m,
                _ => 75m  // default for null or unknown: give benefit of doubt
            };
        }

        public ProgressService(ApplicationDbContext context) 
        { 
          _context = context;   
        }


        public async Task<CourseProgressDTO> CalculateAndSaveAsync(
       int studentId, int courseId)
        {
            // ── STEP 1: Validate student and course exist ────────────
            var student = await _context.Students.FindAsync(studentId);
            var course = await _context.Course.FindAsync(courseId);

            if (student == null)
                throw new InvalidOperationException($"Student {studentId} not found");
            if (course == null)
                throw new InvalidOperationException($"Course {courseId} not found");

            // ── STEP 2: Calculate Homework Score (0-100) ─────────────
            //
            // WHY AsNoTracking? We're just reading, not updating.
            // AsNoTracking = faster, less memory (EF doesn't track changes).
            //
            // HOW homework score works:
            //   We look at homeworks assigned to this course's subjects.
            //   Each homework has a grade in StudentSubjects (Grade field).
            //   We convert letter grades to numbers: A=100, B=80, C=60...
            //   Average them all = homework score.
            //
            // SIMPLIFIED APPROACH for this project:
            //   We check how many homeworks exist for this course
            //   and assume the student completed them (graded by teacher).
            //   In a real system, you'd have a HomeworkSubmission table.
            //
            // WHY this approach?
            //   Your current schema doesn't have homework submission grades.
            //   Homework has Status (Active/Archived) but no student score.
            //   So we use ATTENDANCE to homework sessions as a proxy.

            // Count homeworks for this course (via subjects that belong to this course)
            var homeworksInCourse = await _context.Homeworks
                .AsNoTracking()
                .Where(h => h.Subject != null && h.Subject.CourseId == courseId)
                .CountAsync();

            // For homework score: we use the student's grade from StudentSubjects
            // (the Grade column you have in StudentSubjects.cs)
            var studentGrades = await _context.Set<StudentSubjects>()
                .AsNoTracking()
                .Where(ss => ss.StudentId == studentId &&
                             ss.Subject.CourseId == courseId)
                .Select(ss => ss.Grade)
                .ToListAsync();

            // WHY convert grades after ToListAsync?
            // Remember the hard-won lesson: Path.GetFileName inside Select = null.
            // Same principle: C# methods (ConvertGrade) don't translate to SQL.
            // Load data FIRST, then process in C#.
            decimal homeworkScore = 0;
            if (studentGrades.Count > 0)
            {
                var numericGrades = studentGrades
                    .Select(g => ConvertLetterGrade(g))
                    .ToList();
                homeworkScore = Math.Round(numericGrades.Average(), 1);
            }

            // ── STEP 3: Calculate Attendance Score (0-100) ────────────
            //
            // Formula: (Present days / Total days) × 100
            // Late = 50% credit (came but late)
            // Excused = 100% credit (valid reason)
            // Absent = 0% credit
            var attendances = await _context.Attendances
                .AsNoTracking()
                .Where(a => a.StudentId == studentId)
                .ToListAsync();

            // WHY calculate AFTER ToListAsync?
            // The switch expression (converting Status to score)
            // is C# code — EF can't translate it to SQL.
            decimal attendanceScore = 0;
            int totalDays = attendances.Count;

            if (totalDays > 0)
            {
                // Convert each record to a score (0, 50, or 100)
                decimal totalPoints = attendances.Sum(a => a.Status switch
                {
                    AttendanceStatus.Present => 100m,
                    AttendanceStatus.Late => 50m,
                    AttendanceStatus.Excused => 100m,
                    AttendanceStatus.Absent => 0m,
                    _ => 0m
                });

                attendanceScore = Math.Round(totalPoints / totalDays, 1);
            }

            // ── STEP 4: Calculate Exam Score (0-100) ──────────────────
            var exams = await _context.Exams
                .AsNoTracking()
                .Where(e => e.StudentId == studentId && e.CourseId == courseId)
                .ToListAsync();

            decimal examScore = 0;
            if (exams.Count > 0)
            {
                // Convert each exam to percentage, then average
                // WHY not just average Score directly?
                // Different exams have different MaxScore values.
                // Exam1: 45/50 = 90%, Exam2: 70/100 = 70%
                // Average = (90+70)/2 = 80% ← CORRECT
                // vs. (45+70)/(50+100) = 115/150 = 76.7% ← WRONG for averaging
                var examPercentages = exams
                    .Select(e => e.MaxScore > 0 ? (e.Score / e.MaxScore) * 100 : 0)
                    .ToList();
                examScore = Math.Round(examPercentages.Average(), 1);
            }

            // ── STEP 5: Apply the weights to get OVERALL PROGRESS ─────
            //
            // FORMULA:
            //   Overall = (HomeworkScore × 0.40)
            //           + (AttendanceScore × 0.30)
            //           + (ExamScore × 0.30)
            //
            // Example with Ahmed:
            //   Homework = 85% → 85 × 0.40 = 34
            //   Attendance = 80% → 80 × 0.30 = 24
            //   Exams = 75% → 75 × 0.30 = 22.5
            //   Total = 34 + 24 + 22.5 = 80.5% → "B — Good"
            decimal overallProgress = Math.Round(
                (homeworkScore * HomeworkWeight) +
                (attendanceScore * AttendanceWeight) +
                (examScore * ExamWeight),
                1
            );

            // ── STEP 6: Save or Update in DB ─────────────────────────
            // WHY check if exists first?
            // Remember the unique index on (StudentId, CourseId).
            // If we INSERT a duplicate, SQL will throw an error.
            // So: if exists → UPDATE. If not → INSERT.
            // This pattern is called UPSERT (Update + Insert).
            var existing = await _context.CourseProgress
                .FirstOrDefaultAsync(cp =>
                    cp.StudentId == studentId &&
                    cp.CourseId == courseId);

            CourseProgress progress;

            if (existing != null)
            {
                // UPDATE existing record
                existing.HomeworkScore = homeworkScore;
                existing.AttendanceScore = attendanceScore;
                existing.ExamScore = examScore;
                existing.OverallProgress = overallProgress;
                existing.CalculatedAt = DateTime.UtcNow;
                existing.TotalHomeworks = homeworksInCourse;
                existing.TotalAttendanceDays = totalDays;
                existing.TotalExams = exams.Count;
                progress = existing;
            }
            else
            {
                // INSERT new record
                progress = new CourseProgress
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    HomeworkScore = homeworkScore,
                    AttendanceScore = attendanceScore,
                    ExamScore = examScore,
                    OverallProgress = overallProgress,
                    CalculatedAt = DateTime.UtcNow,
                    TotalHomeworks = homeworksInCourse,
                    TotalAttendanceDays = totalDays,
                    TotalExams = exams.Count
                };
                _context.CourseProgress.Add(progress);
            }

            await _context.SaveChangesAsync();

            // Reload navigation properties for the response DTO
            await _context.Entry(progress).Reference(p => p.Student).LoadAsync();
            await _context.Entry(progress).Reference(p => p.Course).LoadAsync();

            return MapToDTO(progress);
        }


        public async  Task<List<CourseProgressDTO>> GetCourseProgressAsync(int courseId)
        {
            var progressList = await _context.CourseProgress
                .AsNoTracking()
                .Include(cp => cp.Student)
                .Include(cp => cp.Course)
                .Where(cp => cp.CourseId == courseId)
                .OrderByDescending(cp => cp.OverallProgress)
                .ToListAsync();

            return progressList.Select(MapToDTO).ToList();
        }

        public async  Task<CourseProgressDTO> GetProgressAsync(int studentId, int courseId)
        {
            var progress = await _context.CourseProgress
                    .AsNoTracking()
                    .Include(cp => cp.Student)
                    .Include(cp => cp.Course)
                    .FirstOrDefaultAsync(cp =>
                        cp.StudentId == studentId &&
                        cp.CourseId == courseId);

            return progress == null ? null : MapToDTO(progress);
        }

        public async  Task<List<CourseProgressDTO>> GetStudentProgressAsync(int studentId)
        {
            var progressList = await _context.CourseProgress
                 .AsNoTracking()
                 .Include(cp => cp.Student)
                 .Include(cp => cp.Course)
                 .Where(cp => cp.StudentId == studentId )
                 .OrderByDescending(cp => cp.OverallProgress)
                 .ToListAsync();

            return progressList.Select(MapToDTO).ToList();
        }

        private static CourseProgressDTO MapToDTO(CourseProgress cp)
        {
            return new CourseProgressDTO
            {
                Id = cp.Id,
                StudentId = cp.StudentId,
                StudentName = cp.Student != null
                    ? $"{cp.Student.FirstName} {cp.Student.LastName}"
                    : $"Student #{cp.StudentId}",
                CourseId = cp.CourseId,
                CourseName = cp.Course?.Name,
                HomeworkScore = cp.HomeworkScore,
                AttendanceScore = cp.AttendanceScore,
                ExamScore = cp.ExamScore,
                OverallProgress = cp.OverallProgress,
                CalculatedAt = cp.CalculatedAt,
                TotalHomeworks = cp.TotalHomeworks,
                TotalAttendanceDays = cp.TotalAttendanceDays,
                TotalExams = cp.TotalExams
            };
        }

    }
}
