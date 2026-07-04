using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exams");

            builder.HasKey(e => e.Id);


            builder.Property(e=>e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Score)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(e => e.MaxScore)
              .IsRequired()
              .HasColumnType("decimal(5,2)")
              .HasDefaultValue(100);


            builder.Property(e => e.ExamDate).IsRequired();

            builder.Property(e=>e.Notes)
                .HasMaxLength(100)
                .IsRequired(false);

            // Relationships 


            builder.HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasOne(e=>e.Course)
                .WithMany()
                .HasForeignKey(e=>e.CourseId)   
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasData(
               new Exam { Id = 1, StudentId = 1, CourseId = 1, Score = 85, MaxScore = 100, ExamDate = new DateTime(2026, 5, 10) },
               new Exam { Id = 2, StudentId = 2, CourseId = 1, Score = 72, MaxScore = 100, ExamDate = new DateTime(2026, 5, 10) },
               new Exam { Id = 3, StudentId = 3, CourseId = 2, Score = 90, MaxScore = 100, ExamDate = new DateTime(2026, 5, 11) },
               new Exam { Id = 4, StudentId = 1, CourseId = 2, Score = 78, MaxScore = 100, ExamDate = new DateTime(2026, 5, 11) },
               new Exam { Id = 5, StudentId = 4, CourseId = 1, Score = 65, MaxScore = 100, ExamDate = new DateTime(2026, 5, 12) }
           );

        }
    }
}
