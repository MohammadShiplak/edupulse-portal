using System.ComponentModel.DataAnnotations;

namespace School_Project_API.DTO
{
    public class StudentDTO
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="First name is required")]
        [MaxLength(50,ErrorMessage ="First name can not exceed 50 characters")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Last name can not exceed 50 characters")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Date of birth  is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Gender  is required")]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200, ErrorMessage = "address can not exceed 50 characters")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "please provide a valid phone number")]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please provide a valid email address")]
        [EmailAddress(ErrorMessage = "Address cannot exceed 200 characters.")]
        [MaxLength(100)]
        public string Email { get; set; }


    }
}
