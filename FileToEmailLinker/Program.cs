﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FileToEmailLinker.Data;
using FileToEmailLinker.Models.Services.Worker;
using FileToEmailLinker.Models.Services.Schedulation;
using FileToEmailLinker.Models.Services.SchedulationChecker;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FileToEmailLinkerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FileToEmailLinkerContext") ?? throw new InvalidOperationException("Connection string 'FileToEmailLinkerContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHostedService<DailySchedulesReaderService>();

builder.Services.AddTransient<ISchedulationService, SchedulationService>();
builder.Services.AddTransient<ISchedulationChecker,  SchedulationChecker>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
