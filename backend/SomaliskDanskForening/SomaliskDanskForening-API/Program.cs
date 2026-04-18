using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SomaliskDanskForening_Lib.Data;
using SomaliskDanskForening_Lib.Interfaces;
using SomaliskDanskForening_Lib.Repo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ForeningDbContext>(options =>
    options.UseSqlServer(connectionString, b =>
        b.MigrationsAssembly("SomaliskDanskForening-Lib")));

builder.Services.AddScoped<IEventRepo, EventRepositoryDB>();
builder.Services.AddScoped<IDonationRepo, DonationRepositoryDB>();
builder.Services.AddScoped<IContactRepo, ContactRepositoryDB>();

builder.Services.AddSwaggerGen();

// JWT Authentication
var jwtKey = "your-super-secret-key-that-is-at-least-32-characters-long-for-security";
var jwtIssuer = "SomaliskDanskForening";
var jwtAudience = "SomaliskDanskForeningUsers";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Build ALTID efter alle services er registreret
var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();