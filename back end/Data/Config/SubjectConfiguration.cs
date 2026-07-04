using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {

            builder.HasKey(s => s.Id);

            // Configure SubjectName to be required with a maximum length of 100 characters
            builder.Property(s => s.SubjectName)
                   .IsRequired(false) // allows null, since it's nullable
                   .HasMaxLength(100); // maximum length for the string

            // Configure Price with a precision of 18 and scale of 2
            builder.Property(s => s.Price)
                   .HasColumnType("decimal(18,2)"); // 



            // relationships
            /*
             1--M 
            one course includes many subjects 


            */

            builder.HasOne(c => c.Course)
                .WithMany(s => s.Subject)
                .HasForeignKey(f => f.CourseId);

            /*
                         1--M
 One Teacher teaches many subject

             */

            builder.HasOne(t => t.Teacher)
                .WithMany(s => s.Subject)
                .HasForeignKey(f => f.TeacherId);

            builder.HasOne(d=>d.Department)
                .WithMany(s=>s.Subjects)
                .HasForeignKey(f=>f.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);  





            builder.HasData(
    new Subject { Id = 1, SubjectName = "C#", Price = 50, CourseId = 1, TeacherId = 1,DepartmentId=1 },
    new Subject { Id = 2, SubjectName = "EF Core", Price = 60, CourseId = 1, TeacherId = 1, DepartmentId = 2 },
    new Subject { Id = 3, SubjectName = "HTML", Price = 40, CourseId = 2, TeacherId = 2, DepartmentId = 3 },
    new Subject { Id = 4, SubjectName = "CSS", Price = 40, CourseId = 2, TeacherId = 2 , DepartmentId = 4 },
    new Subject { Id = 5, SubjectName = "SQL Basics", Price = 55, CourseId = 3, TeacherId = 4, DepartmentId = 5 },
    new Subject { Id = 6, SubjectName = "Docker Basics", Price = 65, CourseId = 4, TeacherId = 7 , DepartmentId = 6 },
    new Subject { Id = 7, SubjectName = "Azure Fundamentals", Price = 75, CourseId = 5, TeacherId = 9 , DepartmentId = 7 },
    new Subject { Id = 8, SubjectName = "Python Basics", Price = 45, CourseId = 6, TeacherId = 9 , DepartmentId = 8 },
    new Subject { Id = 9, SubjectName = "Ethical Hacking", Price = 90, CourseId = 7, TeacherId = 8 , DepartmentId = 9 },
    new Subject { Id = 10, SubjectName = "Data Structures", Price = 100, CourseId = 10, TeacherId = 10 , DepartmentId = 4 }
);











        }














    }
}
