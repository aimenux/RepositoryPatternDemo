using Example03;
using Example03.Infrastructure;
using Example03.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookRepository, BookRepository>();
if (!builder.Environment.IsContinuousIntegration())
{
    builder.Services.AddDbContext<BookDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("BooksDBConnection"), sqlServerOptions =>
        {
            sqlServerOptions.EnableRetryOnFailure();
        });
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
