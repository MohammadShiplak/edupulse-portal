using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class ClassSubjectConfiguration : IEntityTypeConfiguration<ClassSubject>
    {
      
        public void Configure(EntityTypeBuilder<ClassSubject> builder)
        {
            builder.ToTable("ClassSubjects");

            builder.HasKey(cs => new { cs.ClassId, cs.SubjectId });

            builder.HasOne(cs=>cs.Class)
                .WithMany(cs=>cs.ClassSubjects)
                .HasForeignKey(cs=>cs.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cs => cs.Subject)
            .WithMany(cs => cs.ClassSubjects)
            .HasForeignKey(cs => cs.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Property(cs => cs.AssignedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");




        }
    }
}
