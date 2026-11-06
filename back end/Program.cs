// ✅ All using statements — MUST be first
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using School_Project_API.Services;
using School_Project_API.Services.Interfaces;
using School_Project_API.Hubs;
using School_Project_API.Entities;

using SchoolManagementSystem.API.Services;
using Hangfire;
using Hangfire.SqlServer;
using School_Project_API.options;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// ── DbContext ─────────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var settings = serviceProvider
        .GetRequiredService<IOptions<ConnectionStringsSettings>>()
        .Value;

    options.UseSqlServer(settings.DefaultConnection);
});

builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection("jwt"))
    .Validate(s => !string.IsNullOrEmpty(s.Key), "JWT key is required")
    .ValidateOnStart();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("Email"));

builder.Services.Configure<GeminiSettings>(
    builder.Configuration.GetSection("Gemini"));

builder.Services.Configure<ConnectionStringsSettings>(
    builder.Configuration.GetSection("ConnectionStrings"));



// ── Services ──────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IAccessCardService, AccessCardService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IHomeworkService, HomeworkService>();
builder.Services.AddScoped<IAttendanceAlertService, AttendanceAlertService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<IClassEnrollmentService, ClassEnrollmentService>();
builder.Services.AddScoped<IClassSubjectService, ClassSubjectService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddHttpClient<IGeminiService, GeminiService>();
builder.Services.AddSignalR();



// ── CORS ──────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ── JWT Authentication ────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        };
    });

// ── Controllers + Swagger ─────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new SqlServerStorageOptions
        {
            // WHY these options specifically?
            //   CommandBatchMaxTimeout / QueuePollInterval control how
            //   aggressively Hangfire polls the DB for new jobs. Defaults are
            //   fine for a learning project — don't over-tune prematurely.
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero, // uses SQL Server's Query Notifications (efficient)
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

// WHY a separate server registration?
//   AddHangfire() configures the CLIENT (how to enqueue jobs).
//   AddHangfireServer() starts the actual WORKER that processes the queue.
//   Without this line, jobs get added to the table but NOTHING ever picks them up.
builder.Services.AddHangfireServer();

// Register your new service (same pattern as every other service in Program.cs)
builder.Services.AddScoped<IEmailService, EmailService>();











var app = builder.Build();

// ── Middleware Pipeline ───────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("ReactPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");
app.UseHangfireDashboard("/hangfire");
app.Run();
