using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context; 
        }

        private static DepartmentDTO MapToDto(Department department)
        {

            return new DepartmentDTO
            {

                Id = department.Id,
              Name = department.Name,
            };


        }






        public async Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO departmentDTO)
        {


            var NewDepartment = new Department
            {

             Id= departmentDTO.Id,  
             Name = departmentDTO.Name, 
            };


            _context.Departments.Add(NewDepartment);

            await _context.SaveChangesAsync();





            return MapToDto(NewDepartment);
        }

        public async Task<bool> DeleteDepartemntAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return false;

            _context.Departments.Remove(department);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.AsNoTracking()
                 .Select(d => new DepartmentDTO
                 {

                     Id = d.Id,
                     Name = d.Name,

                 }).ToListAsync();
        }

        public async Task<DepartmentDTO> GetDepartmentByIdAsync(int? id)
        {
        var department=await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                return null;

            return MapToDto(department);

        }

        public async Task<DepartmentStatisticsDTO> GetDepartmentStatisticsAsync(int id)
        { 
         var departmentExists= await _context.Departments
                .AnyAsync(d=>d.Id == id);

            if (!departmentExists)
                return null;

            var departmentName = await _context.Departments
                .Where(d => d.Id == id)
                .Select(n => n.Name)
                .FirstOrDefaultAsync();

            var totalTeachers=await _context.Teacher
                .AsNoTracking()
                .CountAsync(t=>t.DepartmentId == id);   


            var totalSubjects=await _context.Subjects
                .AsNoTracking()
                .CountAsync(d=>d.DepartmentId ==id);


            var totalStudents = await _context.Students
               .AsNoTracking()
               .CountAsync(s => s.DepartmentId == id);

            var totalClasses = await _context.Class
                .AsNoTracking()
                .CountAsync(c => c.DepartmentId == id);


            return new DepartmentStatisticsDTO
            {
                DepartmentId = id,
                DepartmentName = departmentName,
                TotalTeachers = totalTeachers,
                TotalSubjects = totalSubjects,
                TotalClasses = totalClasses,

            };





        }


        public async Task<DepartmentDTO> UpdateDepartmentAsync(int id, DepartmentDTO departmentDTO)
        {

            var department =await  _context.Departments.FindAsync(id);

            if (department == null)
                return null;

            department.Name=departmentDTO.Name;


          await _context.SaveChangesAsync();


            return MapToDto(department);







        }
    }
}
