using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class AccessCardConfiguration : IEntityTypeConfiguration<AccessCard>
    {
        public void Configure(EntityTypeBuilder<AccessCard> builder)
        {


            builder.HasKey(e => e.Id);

            // Configure SerialNo to be of max length 50
            builder.Property(e => e.SerialNo)
                .IsRequired()  // Making SerialNo required
                .HasMaxLength(250);  // Limiting the length of SerialNo

            // Configure ExpirationDate (you could specify column type if necessary)
            builder.Property(e => e.ExpirationDate)
                .IsRequired();


            builder.HasData(LoadAccessCards());

        }

        private List<AccessCard> LoadAccessCards()
        {

            return new List<AccessCard>()
            {

      new AccessCard { Id = 1, SerialNo = "ABCAW", ExpirationDate = new DateTime(2025, 2, 3) },
      new AccessCard { Id = 2, SerialNo = "ABCAX", ExpirationDate = new DateTime(2025, 3, 4) },
      new AccessCard { Id = 3, SerialNo = "ABCBW", ExpirationDate = new DateTime(2025, 4, 5) },
      new AccessCard { Id = 4, SerialNo = "ABCBX", ExpirationDate = new DateTime(2025, 5, 6) },
      new AccessCard { Id = 5, SerialNo = "ABCCA", ExpirationDate = new DateTime(2025, 6, 7) },
      new AccessCard { Id = 6, SerialNo = "ABCCX", ExpirationDate = new DateTime(2025, 7, 8) },
      new AccessCard { Id = 7, SerialNo = "ABCDW", ExpirationDate = new DateTime(2025, 8, 9) },
      new AccessCard { Id = 8, SerialNo = "ABCDX", ExpirationDate = new DateTime(2025, 9, 10) },
      new AccessCard { Id = 9, SerialNo = "ABCDE", ExpirationDate = new DateTime(2025, 10, 11) },
      new AccessCard { Id = 10, SerialNo = "ABCDF", ExpirationDate = new DateTime(2025, 11, 12) },
      new AccessCard { Id = 11, SerialNo = "ABCDG", ExpirationDate = new DateTime(2025, 12, 13) },
      new AccessCard { Id = 12, SerialNo = "ABCDH", ExpirationDate = new DateTime(2026, 1, 14) },
      new AccessCard { Id = 13, SerialNo = "ABCDI", ExpirationDate = new DateTime(2026, 2, 15) }

            };
        }





    }

}





