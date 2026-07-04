using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class StudentSubjectsConfiguration : IEntityTypeConfiguration<StudentSubjects>
    {
        public void Configure(EntityTypeBuilder<StudentSubjects> builder)
        {
            builder.ToTable("StudentSubjects");

            // ─────────────────────────────────────────────────────────────
            // APPROACH 1 — Your current way (separate Id as PK)
            //
            // Use this ONLY if you truly need a single Id column,
            // for example if another table has a FK pointing to this table.
            //
            // ⚠️  Problem: allows the same student to enroll in
            //     the same subject more than once (duplicates).
            //     You would need a manual UNIQUE index to block that.
            // ─────────────────────────────────────────────────────────────

            // builder.HasKey(x => x.Id);

            // Prevent duplicate enrollments manually (required if using Id PK)
            // builder.HasIndex(x => new { x.StudentId, x.SubjectId }).IsUnique();


            // ─────────────────────────────────────────────────────────────
            // APPROACH 2 — Composite Key (recommended ✅)
            //
            // Remove the Id property from StudentSubjects.cs and use this.
            // The combination (StudentId + SubjectId) becomes the PK.
            // The database will automatically block duplicate enrollments.
            // ─────────────────────────────────────────────────────────────

            builder.HasKey(x => new { x.StudentId, x.SubjectId });

            // ── FK: StudentSubjects → Student ────────────────────────────
            // HasOne:        this enrollment record has one Student
            // WithMany:      a Student has many enrollment records
            // HasForeignKey: the FK column in THIS table is StudentId
            // OnDelete:      if a Student is deleted, delete their enrollments too
            // ─────────────────────────────────────────────────────────────
            builder.HasOne(x => x.Student)
                   .WithMany()
                   .HasForeignKey(x => x.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ── FK: StudentSubjects → Subject ────────────────────────────
            // Same pattern — this enrollment belongs to one Subject
            // Restrict: don't allow deleting a Subject that has enrollments
            // ─────────────────────────────────────────────────────────────
            builder.HasOne(x => x.Subject)
                   .WithMany()
                   .HasForeignKey(x => x.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ── Extra columns ─────────────────────────────────────────────
            builder.Property(x => x.EnrollmentDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()"); // SQL Server sets it automatically

            builder.Property(x => x.Grade)
                   .HasMaxLength(5)    // "A", "B+", "C-" etc.
                   .IsRequired(false); // nullable — student may not have a grade yet
        }
    }
}
