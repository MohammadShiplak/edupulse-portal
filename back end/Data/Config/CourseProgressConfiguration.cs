using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class CourseProgressConfiguration : IEntityTypeConfiguration<CourseProgress>
    {
        public void Configure(EntityTypeBuilder<CourseProgress> builder)
        {

            builder.ToTable("CourseProgress");
            builder.HasKey(cp => cp.Id);
            builder.Property(cp => cp.Id).ValueGeneratedOnAdd();

            // WHY decimal(5,2)? Progress like 87.50%
            builder.Property(cp => cp.HomeworkScore).HasColumnType("decimal(5,2)");
            builder.Property(cp => cp.AttendanceScore).HasColumnType("decimal(5,2)");
            builder.Property(cp => cp.ExamScore).HasColumnType("decimal(5,2)");
            builder.Property(cp => cp.OverallProgress).HasColumnType("decimal(5,2)");

            builder.Property(cp => cp.CalculatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // ── Relationships ─────────────────────────────────────────
            builder.HasOne(cp => cp.Student)
                   .WithMany()
                   .HasForeignKey(cp => cp.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cp => cp.Course)
                   .WithMany()
                   .HasForeignKey(cp => cp.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            // WHY unique index on (StudentId, CourseId)?
            // A student can only have ONE progress record per course.
            // This prevents duplicate rows.
            // In SQL it becomes: UNIQUE INDEX on (StudentId, CourseId)
            builder.HasIndex(cp => new { cp.StudentId, cp.CourseId })
                   .IsUnique();
        }
    }
}
