using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.helper;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Services
{
    public class ClassService : IClassService
    {

        private readonly ApplicationDbContext _context;

        public ClassService(ApplicationDbContext context)
        {
_context = context; 

        }



        private ClassDTO MapToDto(Class newclass)
        {

            return new ClassDTO
            {
Id=newclass.Id,   
Name=newclass.Name,
Capacity=newclass.Capacity,
Description=newclass.Description,
            };


        }

        public async Task<ClassDTO> AddClassesAsync(ClassDTO classDTO)
        {
            var NewClass = new Class
            {

                Id = classDTO.Id,
                Name = classDTO.Name,
                Capacity=classDTO.Capacity,
                Description=classDTO.Description,
            };


            _context.Class.Add(NewClass);

            await _context.SaveChangesAsync();





            return MapToDto(NewClass);
        }

        public async  Task<bool> DeleteClassAsync(int id)
        {
            var classId = await _context.Class.FindAsync(id);

            if (classId == null)
                return false;

            _context.Class.Remove(classId);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResponse<ClassDTO>> GetAllClassesAsync(int pageNumber,int pageSize)
        {

            var query = _context.Class.AsNoTracking();

            var totalRecords = await query.CountAsync();


            var classes = await _context.Class

.OrderBy(s => s.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)

        .Select(d => new ClassDTO
        {
            Id = d.Id,
           Name=d.Name,
           Description=d.Description
            

        })
        .ToListAsync();

            return new PagedResponse<ClassDTO>
            {

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling
               (
                    totalRecords / (double)pageSize
                ),

                Data = classes

            };
        }

        public async Task<ClassDTO> GetClassByIdAsync(int id)
        {
            var classId = await _context.Class.FirstOrDefaultAsync(d => d.Id == id);

            if (classId == null)
                return null;

            return MapToDto(classId);
        }

        public async Task<ClassDTO> UpdateClassesAsync(int id, ClassDTO classDTO)
        {
            var updatedclass = await _context.Class.FindAsync(id);

            updatedclass.Name = classDTO.Name;
            updatedclass.Capacity = classDTO.Capacity;
            updatedclass.Description = classDTO.Description;    

            await _context.SaveChangesAsync();


            return MapToDto(updatedclass);


        }
    }
}
