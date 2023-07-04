﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsWebApp.Data;
using NewsWebApp.DataServices;
using Microsoft.Extensions.DependencyInjection;


namespace NewsWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
                  
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            //builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddScoped<IAuthorRepoServiceAPI, AuthorRepoServiceAPI>();
            builder.Services.AddScoped<INewsRepoService, NewsRepoService>();
            builder.Services.AddScoped<IAuthenRepoService, AuthenRepoService>();

            builder.Services.AddHttpClient();
            
            builder.Services.AddLogging();

            builder.Services.AddDistributedMemoryCache(); // Required for session storage
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "token";
                //options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
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
            app.MapRazorPages();
            app.UseRouting();
            app.UseSession();
            

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=News}/{action=Index}/{id?}");

          

            app.Run();
        }
    }
}