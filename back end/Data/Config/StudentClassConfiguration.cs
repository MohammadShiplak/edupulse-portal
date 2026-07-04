using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class StudentClassConfiguration : IEntityTypeConfiguration<StudentClass>
    {
        public void Configure(EntityTypeBuilder<StudentClass> builder)
        {
            builder.ToTable("StudentClasses");

            builder.HasKey(sc=> new {sc.StudentId, sc.ClassId});

            builder.HasOne(sc=>sc.Student)
                .WithMany(s=>s.StudentClasses)
                .HasForeignKey(s=>s.StudentId)  
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sc=>sc.Class)
                .WithMany(c=>c.StudentClasses)
                .HasForeignKey(sc=>sc.ClassId)
                .OnDelete(DeleteBehavior.Restrict);



            builder.Property(sc => sc.EnrolledAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");




        }
    }
}
