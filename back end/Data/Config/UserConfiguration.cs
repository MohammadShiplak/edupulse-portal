using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using School_Project_API.Entities;
using BCrypt.Net;


public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table name
        builder.ToTable("Users");

        // Primary key
        builder.HasKey(u => u.UserId);

        // UserName column
        builder.Property(u => u.UserName)
               .HasMaxLength(50)
               .IsRequired();

        // PasswordHash column
        builder.Property(u => u.PasswordHash)
               .HasMaxLength(255)
               .IsRequired();

        // IsActive column
        builder.Property(u => u.IsActive)
               .HasDefaultValue(true);

        // Email column
        builder.Property(u => u.Email)
               .HasMaxLength(100)
               .IsRequired();

        // Role column
        builder.Property(u => u.Role)
               .HasMaxLength(100)
               .IsRequired();


        builder.Property(u=>u.ProfileImagePath)
            .HasMaxLength(500)
            .IsRequired(false); // This means the column can be NULL, which is appropriate for users who haven't uploaded a profile image.

        // Seed Data
        builder.HasData(

            new User
            {
                UserId = 1,
                UserName = "admin",
                Email = "admin@gmail.com",
                Role = "Admin",
                IsActive = true,
                PasswordHash = "$2a$11$c.4Cmj3rP.Zrpq9PD0ZZveE/aMFE504E9hxqbVVWKesvuDwElKksu"// Admin@123
            },

            new User
            {
                UserId = 2,
                UserName = "teacher1",
                Email = "teacher1@gmail.com",
                Role = "Teacher",
                IsActive = true,
                PasswordHash = "$2a$11$BwOhy/TISJ6FY796dT0I.eN5SALsWuTs6JRp9sRtD0bpXcwEYojtS"//Teacher@123
            },

            new User
            {
                UserId = 3,
                UserName = "teacher2",
                Email = "teacher2@gmail.com",
                Role = "Teacher",
                IsActive = true,
                PasswordHash = "$2a$11$AnlrDcQ1SMmjaQd9WMr8RO1/IC2g9fUdx9TOAMzorCCC7D/OcMHLa"//Teacher@456
            },

            new User
            {
                UserId = 4,
                UserName = "student1",
                Email = "student1@gmail.com",
                Role = "Student",
                IsActive = true,
                PasswordHash = "$2a$11$3Q7ktMT/zai/u.w7PHB7h.2h28yrTIKaLnLDaB0kYfQqAr2ZUSevK"//Student@123
            },

            new User
            {
                UserId = 5,
                UserName = "student2",
                Email = "student2@gmail.com",
                Role = "Student",
                IsActive = false,
                PasswordHash = "$2a$11$vSO5US.IN.S5qC.o4Xy91eU9YnOJ/zhPhEE8hqXP9O1/kgqJv3I4S"//Student@456
            }
        );
    }
}