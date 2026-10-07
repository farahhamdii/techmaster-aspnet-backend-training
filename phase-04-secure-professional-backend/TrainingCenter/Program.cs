
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TrainingCenter.Data;
using TrainingCenter.Entities;
using TrainingCenter.Services;

namespace TrainingCenter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtKey = builder.Configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }

            builder.Services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey)),

                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddControllers();

            builder.Services.AddDbContext<TrainingCenterDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure();
                    }));

            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();
            builder.Services.AddScoped<ITrainingTrackService, TrainingTrackService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IReportService, ReportService>();

            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            builder.Services.AddScoped<
                IPasswordHasher<ApplicationUser>,
                PasswordHasher<ApplicationUser>>();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "Bearer",
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Enter your JWT token."
                    });

                options.AddSecurityRequirement(
                    new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        {
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                            {
                                Reference =
                                    new Microsoft.OpenApi.Models.OpenApiReference
                                    {
                                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            var app = builder.Build();

            // Seed default Admin account
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context =
                    services.GetRequiredService<TrainingCenterDbContext>();

                var passwordHasher =
                    services.GetRequiredService<
                        IPasswordHasher<ApplicationUser>>();

                var adminEmail = "admin@trainingcenter.com";

                var adminExists = await context.Users
                    .AnyAsync(u => u.Email == adminEmail);

                if (!adminExists)
                {
                    var admin = new ApplicationUser
                    {
                        FullName = "System Admin",
                        Email = adminEmail,
                        Role = UserRole.Admin,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    admin.PasswordHash = passwordHasher.HashPassword(
                        admin,
                        "Admin@123");

                    context.Users.Add(admin);

                    await context.SaveChangesAsync();
                }
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}