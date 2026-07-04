using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.helper;

namespace School_Project_API.Services.Interfaces
{
    public class AccessCardService : IAccessCardService
    {
        private readonly ApplicationDbContext _context;
       

public AccessCardService(ApplicationDbContext context)
        {
            _context = context;
        }
            




        private AccessCardDTO MapToDto(AccessCard newcard)
        {

            return new AccessCardDTO
            {
         Id=newcard.Id,
         SerialNo=newcard.SerialNo,
         ExpirationDate=newcard.ExpirationDate
            };


        }



        public async  Task<AccessCardDTO> AddAccessCardAsync(AccessCardDTO cardDTO)
        {
            var NewCard = new AccessCard
            {

                Id = cardDTO.Id,
                SerialNo =cardDTO.SerialNo ,
                ExpirationDate = cardDTO.ExpirationDate,
              
            };


            _context.AccessCards.Add(NewCard);

            await _context.SaveChangesAsync();





            return MapToDto(NewCard);
        }

        public async Task<bool> DeleteAccessAsync(int id)
        {
            var cardId = await _context.AccessCards.FindAsync(id);

            if (cardId == null)
                return false;

            _context.AccessCards.Remove(cardId);

            await _context.SaveChangesAsync();

            return true;
        }

        public async  Task<AccessCardDTO> GetAccessCardByIdAsync(int id)
        {
            var subjectId = await _context.AccessCards.FirstOrDefaultAsync(d => d.Id == id);

            if (subjectId == null)
                return null;

            return MapToDto(subjectId);

        }

        public async Task<PagedResponse<AccessCardDTO>> GetAllCardsAsync(int pageNumber,int pageSize)
        {

            var query = _context.AccessCards.AsNoTracking();

            var totalRecords = await query.CountAsync();


            var cards = await _context.AccessCards

.OrderBy(s => s.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)

        .Select(d => new AccessCardDTO
        {
            Id = d.Id,
            SerialNo = d.SerialNo,
           ExpirationDate=d.ExpirationDate

        })
        .ToListAsync();

            return new PagedResponse<AccessCardDTO>
            {

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling
               (
                    totalRecords / (double)pageSize
                ),

                Data = cards

            };
        }

        public async Task<AccessCardDTO> UpdateAccessAsync(int id, AccessCardDTO cardDTO)
        {
            var updatedcard = await _context.AccessCards.FindAsync(id);

            updatedcard.SerialNo = cardDTO.SerialNo;
            updatedcard.ExpirationDate = cardDTO.ExpirationDate;

            await _context.SaveChangesAsync();


            return MapToDto(updatedcard);
        }
    }
}
