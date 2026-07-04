using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {

            builder.ToTable("Courses");

            builder.HasKey(c => c.Id);

            builder.Property(c=>c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(c=>c.ImagePath)
                .HasMaxLength(500);

            builder.HasData(LoadCourses());

        }


        private static List<Course> LoadCourses()
        {
            return new List<Course>
            {
               new Course { Id = 1, Name = "ASP.NET Core", Price = 100, ImagePath = "/images/asp.png" },
    new Course { Id = 2, Name = "React.js", Price = 120, ImagePath = "/images/react.png" },
    new Course { Id = 3, Name = "SQL Server", Price = 80, ImagePath = "/images/sql.png" },
    new Course { Id = 4, Name = "Docker", Price = 90, ImagePath = "/images/docker.png" },
    new Course { Id = 5, Name = "Azure", Price = 110, ImagePath = "/images/azure.png" },
    new Course { Id = 6, Name = "Python", Price = 95, ImagePath = "/images/python.png" },
    new Course { Id = 7, Name = "Cyber Security", Price = 130, ImagePath = "/images/security.png" },
    new Course { Id = 8, Name = "Machine Learning", Price = 150, ImagePath = "/images/ml.png" },
    new Course { Id = 9, Name = "Networking", Price = 85, ImagePath = "/images/network.png" },
    new Course { Id = 10, Name = "Algorithms", Price = 140, ImagePath = "/images/algo.png" }
            };
        }




    }
}
