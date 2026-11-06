namespace School_Project_API.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendAbsenceEmailAsync(int attendanceId);   
    }
}
