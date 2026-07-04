using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using School_Project_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {


        builder.HasKey(S => S.Id);


        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder.Property(S => S.FirstName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(S => S.LastName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(S => S.DateOfBirth)
            .IsRequired();

        builder.Property(S => S.Gender)
           .IsRequired()
           .HasMaxLength(250);

        builder.Property(S => S.Address)
           .IsRequired()
           .HasMaxLength(250);


        builder.Property(S => S.Email)
          .IsRequired()
          .HasMaxLength(250);

        builder.Property(S => S.Phone)
          .IsRequired()
          .HasMaxLength(250);


        // ── 1:1 ──────────────────────────────────────────────
        // One Student → One AccessCard
        // HasOne/WithOne: both sides have exactly one of the other
        // HasForeignKey<Student>: tells EF which side holds the FK
        // ───────

        // Make CardId nullable
        builder.Property(s => s.CardId)
               .IsRequired(false); // ✅ allows null

        // Make DepartmentId nullable  
        builder.Property(s => s.DepartmentId)
               .IsRequired(false); // ✅ allows null

        builder.HasOne(s => s.AccessCard)
            .WithOne(a => a.Student)
            .HasForeignKey<Student>(s => s.CardId);



        //One to Many 

        builder.HasOne(d => d.Department)
    .WithMany(s => s.Students)
    .HasForeignKey(r => r.DepartmentId);

        /*
        
         Many to Many 
        */

        builder.HasMany(s => s.Subjects)

            .WithMany(s => s.Student)
            .UsingEntity<StudentSubjects>(

              // Configure the Subject side of the bridge table

              j => j.HasOne(ss => ss.Subject)
            .WithMany()
            .HasForeignKey(ss => ss.SubjectId),

            j => j.HasOne(ss => ss.Student)
           .WithMany()
           .HasForeignKey(ss => ss.StudentId)

          


            );

        //builder.Property(S => S.DateOfBirth);


        builder.ToTable("Students");

        builder.HasData(LoadStudents());


        }

    private static List<Student> LoadStudents()
    {
        return new List<Student>
            {
            
    new Student { Id = 1, FirstName = "Mohammad", LastName = "Shiplak", DateOfBirth = new DateTime(2003,3,3), Gender = "Male", Address = "Amman", Email = "m1@test.com", Phone = "111", CardId = 1, DepartmentId = 1 },
    new Student { Id = 2, FirstName = "Ahmad", LastName = "Ali", DateOfBirth = new DateTime(2002,2,2), Gender = "Male", Address = "Irbid", Email = "m2@test.com", Phone = "222", CardId = 2, DepartmentId = 2 },
    new Student { Id = 3, FirstName = "Sara", LastName = "Khaled", DateOfBirth = new DateTime(2001,1,1), Gender = "Female", Address = "Zarqa", Email = "m3@test.com", Phone = "333", CardId = 3, DepartmentId = 3 },
    new Student { Id = 4, FirstName = "Lina", LastName = "Hasan", DateOfBirth = new DateTime(2000,4,4), Gender = "Female", Address = "Aqaba", Email = "m4@test.com", Phone = "444", CardId = 4, DepartmentId = 4 },
    new Student { Id = 5, FirstName = "Omar", LastName = "Salem", DateOfBirth = new DateTime(1999,5,5), Gender = "Male", Address = "Madaba", Email = "m5@test.com", Phone = "555", CardId = 5, DepartmentId = 5 },
    new Student { Id = 6, FirstName = "Rami", LastName = "Naser", DateOfBirth = new DateTime(2003,6,6), Gender = "Male", Address = "Salt", Email = "m6@test.com", Phone = "666", CardId = 6, DepartmentId = 6 },
    new Student { Id = 7, FirstName = "Noor", LastName = "Sami", DateOfBirth = new DateTime(2002,7,7), Gender = "Female", Address = "Karak", Email = "m7@test.com", Phone = "777", CardId = 7, DepartmentId = 7 },
    new Student { Id = 8, FirstName = "Yousef", LastName = "Adel", DateOfBirth = new DateTime(2001,8,8), Gender = "Male", Address = "Jerash", Email = "m8@test.com", Phone = "888", CardId = 8, DepartmentId = 8 },
    new Student { Id = 9, FirstName = "Mona", LastName = "Ahmad", DateOfBirth = new DateTime(2000,9,9), Gender = "Female", Address = "Ajloun", Email = "m9@test.com", Phone = "999", CardId = 9, DepartmentId = 9 },
    new Student { Id = 10, FirstName = "Khaled", LastName = "Jamal", DateOfBirth = new DateTime(1998,10,10), Gender = "Male", Address = "Mafraq", Email = "m10@test.com", Phone = "1010", CardId = 10, DepartmentId = 10 }


            };
    }















}

