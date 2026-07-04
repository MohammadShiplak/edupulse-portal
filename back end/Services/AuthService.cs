using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace School_Project_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _environment;
        public AuthService(ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;


        }





        // ─────────────────────────────────────────────────────────────────────────
        public async Task<string> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
                return null;

            if (!user.IsActive)
                return null;

            // ❓ What is a "Claim"?
            // A claim is a KEY-VALUE pair stored inside the JWT token.
            // Think of it like a passport: it has your name, nationality, birth date.
            // We put user info INTO the token so the frontend can READ it after login.
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.UserName),

                // ✅ NEW CLAIM: We embed the image path in the token.
                // Why? So the frontend can get the image path by just decoding
                // the JWT — no extra API call needed!
                // We use a custom claim name "profileImage".
                // If no image, we store an empty string (never null, JWT doesn't like null).
                new Claim("profileImage", user.ProfileImagePath ?? "")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // REGISTER — now handles image upload
        // ─────────────────────────────────────────────────────────────────────────
        public async Task<bool> RegisterAsync(RegisterDTO registerDTO)
        {
            // Step 1: Check if email is already used
            var exists = await _context.Users.AnyAsync(u => u.Email == registerDTO.Email);
            if (exists)
                return false;

            // ─────────────────────────────────────────────────────────────────
            // Step 2: Handle the profile image upload
            // ─────────────────────────────────────────────────────────────────

            // This will store the RELATIVE path like "avatars/abc123.jpg"
            // We store relative, not absolute, because the server folder
            // location changes between dev/production machines.
            string? profileImagePath = null;

            // ❓ registerDTO.ProfileImage != null means the user actually selected a file.
            // Length > 0 guards against an empty (broken) upload.
            if (registerDTO.ProfileImage != null && registerDTO.ProfileImage.Length > 0)
            {
                // ── Safety Check: Only allow image files ──────────────────────
                // Why? If we skip this, a hacker could upload a .exe or .php file!
                // We check both the extension AND the content-type header.
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(registerDTO.ProfileImage.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return false; // reject non-image files

                // ── Build the folder path where we'll save the file ──────────
                // _environment.WebRootPath = the full path to "wwwroot" folder
                // Example: "C:/MyProject/wwwroot"
                // We create a subfolder: "wwwroot/avatars/"
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "avatars");

                // ❓ Directory.CreateDirectory: If the "avatars" folder doesn't
                // exist yet, create it. If it does exist, do nothing. Safe either way.
                Directory.CreateDirectory(uploadsFolder);

                // ── Generate a unique filename ────────────────────────────────
                // ❓ Why unique? If two people both upload "photo.jpg", they'd
                // overwrite each other's file! So we use a GUID (random unique ID).
                // Example result: "a3f1bc29-4d0e-4a8f-9b2e-12345678.jpg"
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";

                // ── Build the full disk path for saving ──────────────────────
                // Example: "C:/MyProject/wwwroot/avatars/a3f1bc29....jpg"
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // ── Actually save the file to disk ────────────────────────────
                // CopyToAsync reads the uploaded bytes and writes them to our file.
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await registerDTO.ProfileImage.CopyToAsync(fileStream);
                }

                // ── Store only the RELATIVE path in the database ──────────────
                // We store "avatars/a3f1bc29....jpg" (not the full disk path).
                // The frontend will request: http://localhost:5000/avatars/filename.jpg
                // ASP.NET serves files from wwwroot automatically via UseStaticFiles().
                profileImagePath = $"avatars/{uniqueFileName}";
            }

            // Step 3: Create the user and save to database
            var newUser = new User
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                Role = registerDTO.Role,
                IsActive = registerDTO.IsActive,
                ProfileImagePath = profileImagePath   // ✅ could be null if no image
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
    



