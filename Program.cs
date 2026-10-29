using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var databaseConfiguration = new DatabaseConfiguration();

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(databaseConfiguration.ConnectionString));

using var host = builder.Build();