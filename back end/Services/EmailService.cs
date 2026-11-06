
  using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.EntityFrameworkCore;
using School_Project_API.Services.Interfaces;
using Microsoft.Extensions.Options;
using School_Project_API.options;

namespace School_Project_API.Services
    {
        public class EmailService : IEmailService
        {
            private readonly ApplicationDbContext _context;
            private readonly EmailSettings _emailSettings;
            private readonly ILogger<EmailService> _logger;

            public EmailService(
                ApplicationDbContext context,
                IOptions<EmailSettings>emailOptions,
                ILogger<EmailService> logger)
            {
                _context = context;
                _emailSettings = emailOptions.Value;
                _logger = logger;
            }

            public async Task SendAbsenceEmailAsync(int attendanceId)
            {
                // ── STEP 1: Re-fetch fresh data (see explanation above) ──────
                var attendance = await _context.Attendances
                    .Include(a => a.Student)
                    .AsNoTracking() // we're only reading, never saving this entity
                    .FirstOrDefaultAsync(a => a.Id == attendanceId);

                // WHY these guard clauses instead of assuming data exists?
                //   This job might run MINUTES after it was queued (Hangfire retries,
                //   server restarts, etc). By the time it runs, the attendance record
                //   or student could have been deleted. A background job must NEVER
                //   assume the world looks the same as when it was queued.
                if (attendance?.Student == null)
                {
                    _logger.LogWarning(
                        "Attendance {Id} or its Student no longer exists. Skipping email.",
                        attendanceId);
                    return; // don't throw — throwing would trigger a Hangfire RETRY,
                            // and retrying won't fix "the record doesn't exist"
                }

                if (string.IsNullOrWhiteSpace(attendance.Student.ParentEmail))
                {
                    _logger.LogInformation(
                        "Student {Id} has no ParentEmail on file. Skipping email.",
                        attendance.Student.Id);
                    return; // same reasoning — retrying won't add an email address
                }

                // ── STEP 2: Build the email ───────────────────────────────────
                var studentName = $"{attendance.Student.FirstName} {attendance.Student.LastName}";
                var dateLabel = attendance.Date?.ToString("MMMM dd, yyyy") ?? "today";

            var fromAddress = _emailSettings.FromAddress;
            var fromName = string.IsNullOrWhiteSpace(_emailSettings.FromName)
                ? fromAddress
                : _emailSettings.FromName;

            var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromAddress));
                message.To.Add(MailboxAddress.Parse(attendance.Student.ParentEmail));
                message.Subject = $"Absence Notice — {studentName}";
                message.Body = new TextPart("plain")
                {
                    Text = $"Dear Parent/Guardian,\n\n" +
                           $"This is to inform you that {studentName} was marked ABSENT " +
                           $"on {dateLabel}.\n\n" +
                           $"If you believe this is an error, please contact the school office.\n\n" +
                           $"— EduPulse Portal"
                };

                // ── STEP 3: Send via SMTP ─────────────────────────────────────
                using var client = new SmtpClient();

                try
                {
                    await client.ConnectAsync(
                        _emailSettings.SmtpHost,
                        _emailSettings.SmtpPort,
                        MailKit.Security.SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(
                 _emailSettings.Username,
                 _emailSettings.Password);

                    await client.SendAsync(message);

                    _logger.LogInformation(
                        "Absence email sent to {Email} for student {Id}",
                        attendance.Student.ParentEmail, attendance.Student.Id);
                }
                finally
                {
                    // WHY finally, not just calling this after SendAsync?
                    //   If SendAsync throws, we still want to cleanly close the
                    //   connection instead of leaking an open socket.
                    await client.DisconnectAsync(true);
                }

                // NOTE: We deliberately DON'T catch the exception here and swallow it.
                // WHY? If SMTP genuinely fails (server down, bad credentials), we WANT
                // Hangfire to catch that exception and automatically RETRY the job
                // later (Hangfire's default retry policy: several attempts with
                // increasing delay). Swallowing the error here would silently lose
                // the notification forever.
            }
        }
    }





