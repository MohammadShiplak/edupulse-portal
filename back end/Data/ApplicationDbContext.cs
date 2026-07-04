using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using School_Project_API.Entities;

public class ApplicationDbContext : DbContext
{
    // ── DbSets (your tables) ──────────────────────────────────
    public DbSet<Student> Students { get; set; }
    public DbSet<AccessCard> AccessCards { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Class> Class { get; set; }
    public DbSet<Teacher> Teacher { get; set; }
    public DbSet<Course> Course { get; set; }
    public DbSet<Subject> Subjects { get; set; }

    public DbSet<StudentClass> StudentClasses { get; set; }

    public DbSet<ClassSubject> ClassSubjects { get; set; }
    public DbSet<AttendanceAlert> AttendanceAlerts { get; set; }
    public DbSet<Homework> Homeworks { get; set; }  
    public DbSet<Attendance> Attendances { get; set; }  

    public DbSet<Exam> Exams { get; set; }
    public DbSet<CourseProgress>CourseProgress { get; set; }



    // ── Constructor (receives connection string from Program.cs) ──
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ── REMOVE OnConfiguring completely ──────────────────────
    // Program.cs already handles the connection string via DI
    // Having both causes conflicts

    // ── Entity Configurations ─────────────────────────────────
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}

