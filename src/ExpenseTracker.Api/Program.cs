using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ExpenseTracker.Api.Filters;
using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Application.Dtos.Input;
using ExpenseTracker.Application.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
    options.UseSqlite("Data Source=expenseTracker.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Validators
builder.Services.AddScoped<ValidationFilter<CreateExpenseDto>>();
builder.Services.AddScoped<ValidationFilter<CreateCategoryDto>>();
builder.Services.AddScoped<ValidationFilter<GetExpensesInputDto>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();