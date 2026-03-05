using Microsoft.EntityFrameworkCore;
using SomaliskDanskForening_Lib;
using SomaliskDanskForening_Lib.Data;
using SomaliskDanskForening_Lib.Interfaces;
using SomaliskDanskForening_Lib.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DbContext
builder.Services.AddDbContext<ForeningDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register repositories
builder.Services.AddScoped<IEventRepo, EventRepositoryDB>();
builder.Services.AddScoped<IDonationRepo, DonationRepositoryDB>();
builder.Services.AddScoped<IContactRepo, ContactRepositoryDB>();

builder.Services.AddSwaggerGen();

// CORS: dev-friendly policy (consider restricting origins for production)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// IMPORTANT: apply CORS before HTTPS redirection so preflight/redirect responses include CORS headers
app.UseCors();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    
app.UseAuthorization();

app.MapControllers();

app.Run();



