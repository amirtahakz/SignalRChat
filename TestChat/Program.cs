using CoreLayer.Services.Chats.ChatGroups;
using CoreLayer.Services.Chats;
using CoreLayer.Services.Roles;
using CoreLayer.Services.Users.UserGroups;
using CoreLayer.Services.Users;
using DataLayer.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.

services.AddControllersWithViews();

// For Entity Framework  
services.AddDbContext<ApplicationDbContext>(option => { option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });


services.AddScoped<IChatGroupService, ChatGroupService>();
services.AddScoped<IChatService, ChatService>();

services.AddScoped<IRoleService, RoleService>();

services.AddScoped<IUserService, UserService>();
services.AddScoped<IUserGroupService, UserGroupService>();

services.AddSignalR();

services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(option =>
{
    option.LoginPath = "/auth";
    option.LogoutPath = "/auth/Logout";
    option.ExpireTimeSpan = TimeSpan.FromDays(30);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();