using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;
using System.Reflection.Metadata.Ecma335;

namespace School_Project_API.Data.Config
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(250);

            

            builder.Property(c => c.Specialization)
                   .IsRequired();

            builder.Property(c => c.HireDate)
                   .IsRequired();


            /*
             1-M one department has many teachers
            */

            builder.HasOne(d => d.Department)
                .WithMany(t => t.Teachers)
                .HasForeignKey(f => f.DepartmentId)
                .IsRequired(false);
           











            builder.ToTable("Teachers");

            // Use fixed dates for seed data to avoid EF thinking it's always changed
            builder.HasData(
              new Teacher { Id = 1, Name = "Ali Ahmad", Specialization = "Backend", HireDate = new DateTime(2020, 1, 1), DepartmentId = 1 },
    new Teacher { Id = 2, Name = "Sara Khaled", Specialization = "Frontend", HireDate = new DateTime(2021, 2, 1), DepartmentId = 1 },
    new Teacher { Id = 3, Name = "Omar Sami", Specialization = "HR", HireDate = new DateTime(2019, 3, 1), DepartmentId = 2 },
    new Teacher { Id = 4, Name = "Lina Hasan", Specialization = "Finance", HireDate = new DateTime(2022, 4, 1), DepartmentId = 3 },
    new Teacher { Id = 5, Name = "Ahmad Naser", Specialization = "Marketing", HireDate = new DateTime(2020, 5, 1), DepartmentId = 4 },
    new Teacher { Id = 6, Name = "Yousef Adel", Specialization = "Sales", HireDate = new DateTime(2018, 6, 1), DepartmentId = 5 },
    new Teacher { Id = 7, Name = "Mona Ali", Specialization = "Operations", HireDate = new DateTime(2023, 7, 1), DepartmentId = 6 },
    new Teacher { Id = 8, Name = "Khaled Jamal", Specialization = "Cyber Security", HireDate = new DateTime(2021, 8, 1), DepartmentId = 7 },
    new Teacher { Id = 9, Name = "Rami Saeed", Specialization = "AI", HireDate = new DateTime(2019, 9, 1), DepartmentId = 8 },
    new Teacher { Id = 10, Name = "Noor Hasan", Specialization = "Networking", HireDate = new DateTime(2022, 10, 1), DepartmentId = 9 }

            );
        }
    }




}
