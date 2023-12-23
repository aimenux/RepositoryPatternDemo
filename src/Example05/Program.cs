using Example05;
using Example05.Infrastructure;
using Example05.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
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