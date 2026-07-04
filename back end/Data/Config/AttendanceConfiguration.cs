using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{

    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendances");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.StudentId)
                   .IsRequired(false);

            builder.Property(a => a.Date)
                   .IsRequired(false);

            builder.Property(a => a.Status)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(a => a.Notes)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.HasOne(a => a.Student)
                   .WithMany()
                   .HasForeignKey(a => a.StudentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(a => new { a.StudentId, a.Date })
                   .IsUnique()
                   .HasFilter("[StudentId] IS NOT NULL AND [Date] IS NOT NULL");

            builder.HasData(LoadAttendances());
        }

        private static List<Attendance> LoadAttendances()
        {
            return new List<Attendance>
            {

    new Attendance { Id = 1, StudentId = 1, Date = new DateTime(2026,5,1), Status = AttendanceStatus.Present, Notes = "On time" },
    new Attendance { Id = 2, StudentId = 2, Date = new DateTime(2026,5,1), Status = AttendanceStatus.Absent, Notes = "Sick" },
    new Attendance { Id = 3, StudentId = 3, Date = new DateTime(2026,5,1), Status = AttendanceStatus.Late, Notes = "Traffic" },
    new Attendance { Id = 4, StudentId = 4, Date = new DateTime(2026,5,1), Status = AttendanceStatus.Present, Notes = "Excellent" },
    new Attendance { Id = 5, StudentId = 5, Date = new DateTime(2026,5,1), Status = AttendanceStatus.Excused, Notes = "Medical excuse" },
    new Attendance { Id = 6, StudentId = 6, Date = new DateTime(2026,5,2), Status = AttendanceStatus.Present, Notes = "On time" },
    new Attendance { Id = 7, StudentId = 7, Date = new DateTime(2026,5,2), Status = AttendanceStatus.Absent, Notes = "Travel" },
    new Attendance { Id = 8, StudentId = 8, Date = new DateTime(2026,5,2), Status = AttendanceStatus.Present, Notes = "Good" },
    new Attendance { Id = 9, StudentId = 9, Date = new DateTime(2026,5,2), Status = AttendanceStatus.Late, Notes = "Late bus" },
    new Attendance { Id = 10, StudentId = 10, Date = new DateTime(2026,5,2), Status = AttendanceStatus.Present, Notes = "Participated" }



    };

                }

    }

}

