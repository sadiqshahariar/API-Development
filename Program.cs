using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newroz_Home_Task.Models;
using Newroz_Home_Task.Extensions;

var builder = WebApplication.CreateBuilder(args);


//database connection

var Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            .Build();



builder.Services.AddDbContext<NewrozdbContext>(options =>
    options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection"))));

//DI intregate
builder.Services.DIService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
