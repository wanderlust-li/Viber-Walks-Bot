using Viber_bot.Models;
using Microsoft.EntityFrameworkCore;

public class WalkDbContext : DbContext
{
    // параметр options, який визначає налаштування для з'єднання з базою даних. 
    public WalkDbContext(DbContextOptions<WalkDbContext> options) : base(options)
    {
    }
    
    public DbSet<Walk> Walks { get; set; }
}