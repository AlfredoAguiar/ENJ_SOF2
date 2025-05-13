using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Dbes2>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Host=localhost;Database=es2;Username=postgres;Password=es2")));

builder.Services.AddScoped<IPasswordHasher<Utilizadore>, PasswordHasher<Utilizadore>>();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();


app.UseRouting();


app.MapControllers();

app.Run();

