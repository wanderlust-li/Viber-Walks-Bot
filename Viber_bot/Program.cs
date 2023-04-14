using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<WalkDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

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

// Запускаємо ngrok
var ngrokProcess = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = "ngrok",
        Arguments = $"http {app.Urls.FirstOrDefault()}",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        CreateNoWindow = true
    }
};
ngrokProcess.Start();

app.Run();

// Зупиняємо ngrok після закриття програми
ngrokProcess.CloseMainWindow();
ngrokProcess.Kill();