namespace School_Project_API.DTO
{
    public class RegisterDTO
    {
        public string UserName { get; set; }    

        public string Password { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }

        public IFormFile? ProfileImage { get; set; } // Optional profile image upload   
    }
}
