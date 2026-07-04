using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.helper;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Services
{
    public class SubjectService:ISubjectService
    {
        private readonly ApplicationDbContext _context;



        public SubjectService(ApplicationDbContext context)
        {
            _context = context;
        }



        private SubjectDTO MapToDto(Subject newSubject)
        {

            return new SubjectDTO
            {
              Id=newSubject.Id,
              SubjectName=newSubject.SubjectName,
              Price=newSubject.Price,
            };


        }

   
        public async Task<SubjectDTO> GetSubjectByIdAsync(int id)
        {
            var subjectId = await _context.Subjects.FirstOrDefaultAsync(d => d.Id == id);

            if (subjectId == null)
                return null;

            return MapToDto(subjectId);
        }

    
        public async Task<SubjectDTO> AddSubjectAsync(SubjectDTO subjectTO)
        {
            var NewSubject = new Subject
            {

                Id = subjectTO.Id,
                SubjectName = subjectTO.SubjectName,
                Price = subjectTO.Price,
              
            };


            _context.Subjects.Add(NewSubject);

            await _context.SaveChangesAsync();





            return MapToDto(NewSubject);
        }

        public async Task<SubjectDTO> UpdateSubjectAsync(int id,SubjectDTO subjectDTO)
        {
            var updatedSubject = await _context.Subjects.FindAsync(id);

         
            updatedSubject.SubjectName = subjectDTO.SubjectName;
            updatedSubject.Price = subjectDTO.Price;

           
      
            await _context.SaveChangesAsync();


            return MapToDto(updatedSubject);
        }

        public async  Task<bool> DeleteSubjectAsync(int id)
        {
            var subjectId = await _context.Subjects.FindAsync(id);

            if (subjectId == null)
                return false;

            _context.Subjects.Remove(subjectId);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResponse<SubjectDTO>> GetAllSubjectAsync(int pageNumber, int pageSize)
        {
            var query = _context.Subjects.AsNoTracking();

            var totalRecords = await query.CountAsync();

            var subjects = await _context.Subjects

.OrderBy(s => s.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)

        .Select(d => new SubjectDTO
        {
            Id = d.Id,
            SubjectName = d.SubjectName,
            Price = d.Price,


        })
        .ToListAsync();

            return new PagedResponse<SubjectDTO>
            {

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling
               (
                    totalRecords / (double)pageSize
                ),

                Data = subjects

            };
        }
    }
}
