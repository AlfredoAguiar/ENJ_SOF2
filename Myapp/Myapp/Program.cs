using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Myapp;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient(); 
builder.Services.AddScoped<UserStateService>();
var app = builder.Build();

// Configurar o pipeline de requisição HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();