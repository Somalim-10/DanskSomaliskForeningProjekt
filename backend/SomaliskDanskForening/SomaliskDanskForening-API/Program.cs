using Microsoft.EntityFrameworkCore;
using SomaliskDanskForening_Lib;
using SomaliskDanskForening_Lib.Data;
using SomaliskDanskForening_Lib.Interfaces;
using SomaliskDanskForening_Lib.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//et objekt gennem hele applikationens leve
//builder.Services.AddSingleton<PantsRepository>();
// opretter et pair request
///builder.Services.AddDbContext<>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DbContext (uses OnConfiguring with connection string in ForeningDbContext)
builder.Services.AddDbContext<ForeningDbContext>(options =>
    options.UseSqlServer(connectionString));


// Register repositories
builder.Services.AddScoped<IEventRepo, EventRepositoryDB>();
builder.Services.AddScoped<IDonationRepo, DonationRepositoryDB>();
builder.Services.AddScoped<IContactRepo, ContactRepositoryDB>();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
var app = builder.Build();




// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();



