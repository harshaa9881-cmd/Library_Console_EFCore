
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Menus;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Database configuration
var databaseConfiguration = new DatabaseConfiguration();

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(databaseConfiguration.ConnectionString));

// Register repositories
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<BookIssueRepository>();
builder.Services.AddScoped<AuditLogRepository>();

// Register services
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<BookIssueService>();
builder.Services.AddScoped<AuditLogService>();

// Register menus
builder.Services.AddScoped<BookMenu>();
builder.Services.AddScoped<MemberMenu>();
builder.Services.AddScoped<BookIssueMenu>();

using var host = builder.Build();
using var scope = host.Services.CreateScope();

var bookMenu = scope.ServiceProvider.GetRequiredService<BookMenu>();
var memberMenu = scope.ServiceProvider.GetRequiredService<MemberMenu>();
var bookIssueMenu = scope.ServiceProvider.GetRequiredService<BookIssueMenu>();

while (true)
{
    Console.WriteLine("\n===== LIBRARY MANAGEMENT SYSTEM =====");
    Console.WriteLine("1. Book Menu");
    Console.WriteLine("2. Member Menu");
    Console.WriteLine("3. Book Issue Menu");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");

    string choice = Console.ReadLine() ?? "";

    switch (choice)
    {
        case "1":
            await bookMenu.ShowMenuAsync();
            break;

        case "2":
            await memberMenu.ShowMenuAsync();
            break;

        case "3":
            await bookIssueMenu.ShowMenuAsync();
            break;

        case "0":
            Console.WriteLine("Exiting Library Management System.");
            return;

        default:
            Console.WriteLine("Invalid choice. Try again.");
            break;
    }
}
