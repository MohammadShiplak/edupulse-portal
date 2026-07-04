namespace School_Project_API.Entities
{
    public class User
    {

        public int UserId { get; set; }

        public bool IsActive { get; set; }
        public string UserName { get; set; }

        // Authentication related fields
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        
        public string Role { get; set; }  
     
        public string? ProfileImagePath { get; set; }   
    }
}
