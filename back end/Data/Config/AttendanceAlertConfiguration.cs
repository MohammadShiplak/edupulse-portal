using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class AttendanceAlertConfiguration : IEntityTypeConfiguration<AttendanceAlert>
    {
        public void Configure(EntityTypeBuilder<AttendanceAlert> builder)
        {
            builder.ToTable("AttendanceAlerts");

            builder.HasKey(a => a.Id);

            builder.Property(a=>a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.StudentId)
                   .IsRequired(false);

            builder.Property(a => a.ConsecutiveAbsences)
                   .IsRequired();   

            builder.Property(a => a.AlertDate)
                   .IsRequired();   


            builder.Property(a => a.Status)
                   .IsRequired()
                   .HasConversion<int>();   

            builder.HasOne(a => a.Student)
                   .WithMany()
                   .HasForeignKey(a => a.StudentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(x => x.StudentId);

            builder.HasIndex(x => x.Status);


            builder.HasIndex(x => x.AlertDate);



        }
    }
}
