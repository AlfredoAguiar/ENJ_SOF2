using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext with Npgsql for PostgreSQL
builder.Services.AddDbContext<Dbes2>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Host=localhost;Database=es2;Username=postgres;Password=es2")));

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
