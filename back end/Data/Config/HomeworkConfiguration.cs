using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {
     builder.ToTable("Homeworks");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(h => h.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(h => h.Description)
                   .HasMaxLength(2000)
                   .IsRequired(false);

            builder.Property(h => h.DueDate)
                   .IsRequired();

            builder.Property(h => h.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(h => h.Status)
                   .IsRequired()
                   .HasConversion<int>();


            builder.Property(h => h.FilePath)
                        .HasMaxLength(500)
                        .IsRequired(false); 


            // ── RELATIONSHIPS ─────────────────────────────────────────── 

            // teacher can assign many homeworks, but each homework has one teacher

            builder.HasOne(h => h.Teacher)
                   .WithMany()
                   .HasForeignKey(h => h.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict); // if teacher is deleted, delete their homeworks


            // class can have many homeworks, but each homework is assigned to one class (nullable) 
            builder.HasOne(h => h.Class)
                   .WithMany()
                   .HasForeignKey(h => h.ClassId)
                   .IsRequired(false)   
                   .OnDelete(DeleteBehavior.NoAction); // if class is deleted, set ClassId to null   


           builder.HasOne(h => h.Subject)
                   .WithMany()
                   .HasForeignKey(h => h.SubjectId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.NoAction); // if subject is deleted, set SubjectId to null



            builder.HasIndex(h => h.TeacherId);
            builder.HasIndex(h => h.DueDate);


            builder.HasData(
    new Homework
    {
        Id = 1,
        Title = "Math Algebra Practice",
        Description = "Complete exercises 1 to 15 from the algebra worksheet.",
        DueDate = new DateTime(2026, 6, 5),
        CreatedAt = new DateTime(2026, 6, 1),
        Status = HomeworkStatus.Active,
        TeacherId = 1,
        ClassId = 1,
        SubjectId = 1,
        FilePath = null,


    },
    new Homework
    {
        Id = 2,
        Title = "Science Lab Report",
        Description = "Write a lab report about the plant growth experiment.",
        DueDate = new DateTime(2026, 6, 7),
        CreatedAt = new DateTime(2026, 6, 1),
        Status = HomeworkStatus.Active,
        TeacherId = 1,
        ClassId = 2,
        SubjectId = 2,
        FilePath = null,
    },
    new Homework
    {
        Id = 3,
        Title = "English Reading Summary",
        Description = "Read chapter 4 and write a one-page summary.",
        DueDate = new DateTime(2026, 6, 8),
        CreatedAt = new DateTime(2026, 6, 1),
        Status = HomeworkStatus.Archived,
        TeacherId = 2,
        ClassId = 1,
        SubjectId = 3,
        FilePath=null,
    },
    new Homework
    {
        Id = 4,
        Title = "History Timeline",
        Description = "Create a timeline of key events from the lesson.",
        DueDate = new DateTime(2026, 6, 10),
        CreatedAt = new DateTime(2026, 6, 1),
        Status = HomeworkStatus.Archived,
        TeacherId = 2,
        ClassId = 3,
        SubjectId = 4,
        FilePath = null,
    },
    new Homework
    {
        Id = 5,
        Title = "Geography Map Activity",
        Description = "Label the countries and capitals on the provided map.",
        DueDate = new DateTime(2026, 6, 11),
        CreatedAt = new DateTime(2026, 6, 1),
        Status = HomeworkStatus.Active,
        TeacherId = 3,
        ClassId = 2,
        SubjectId = 5,
        FilePath = null,
    },
    new Homework
    {
        Id = 6,
        Title = "Computer Basics Assignment",
        Description = "Answer the questions about hardware and software.",
        DueDate = new DateTime(2026, 6, 12),
        CreatedAt = new DateTime(2026, 6, 1),
        Status = HomeworkStatus.Active,
        TeacherId = 3,
        ClassId = 4,
        SubjectId = 6,
        FilePath = null,
    },
    new Homework
    {
        Id = 7,
        Title = "Arabic Grammar Practice",
        Description = "Complete the grammar exercises from page 32.",
        DueDate = new DateTime(2026, 6, 13),
        CreatedAt = new DateTime(2026, 6, 1),
        Status = HomeworkStatus.Archived,
        TeacherId = 4,
        ClassId = 3,
        SubjectId = 7,
        FilePath = null,
    },
    new Homework
    {
        Id = 8,
        Title = "Art Sketch Assignment",
        Description = "Draw a still-life sketch using pencil shading.",
        DueDate = new DateTime(2026, 6, 14),
        CreatedAt = new DateTime(2026, 6, 1),
        Status = HomeworkStatus.Active,
        TeacherId = 4,
        ClassId = 4,
        SubjectId = 8
    },
    new Homework
    {
        Id = 9,
        Title = "Physical Education Reflection",
        Description = "Write a short reflection about teamwork in sports.",
        DueDate = new DateTime(2026, 6, 15),
        CreatedAt = new DateTime(2026, 6, 1),
        Status = HomeworkStatus.Active,
        TeacherId = 5,
        ClassId = 5,
        SubjectId = 9,
        FilePath = null,
    },
    new Homework
    {
        Id = 10,
        Title = "Final Revision Worksheet",
        Description = "Complete the revision worksheet before next class.",
        DueDate = new DateTime(2026, 6, 16),
        CreatedAt = new DateTime(2026, 6, 1),
        Status = HomeworkStatus.Active,
        TeacherId = 5,
        ClassId = 5,
        SubjectId = 10,
        FilePath = null,
    }
);










        }
    }
}
