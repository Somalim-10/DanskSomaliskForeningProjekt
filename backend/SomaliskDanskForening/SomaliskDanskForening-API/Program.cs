using SomaliskDanskForening_Lib;
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

builder.Services.AddSingleton<IEventRepo, EventRepositoryDB>();


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
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();



