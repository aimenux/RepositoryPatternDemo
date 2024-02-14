using Example01;
using Example01.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();