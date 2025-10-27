
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SistemaHotelAloha.AccesoDatos;
using SistemaHotelAloha.Web.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();         // Necesario para _Host.cshtml
builder.Services.AddServerSideBlazor();   // Blazor Server

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Si luego agregas auth:
// app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapRazorPages();                 // <- publica las Razor Pages (incluye _Host)
app.MapFallbackToPage("/_Host");     // <- fallback a _Host (NO controllers, NO index.html)

app.Run();