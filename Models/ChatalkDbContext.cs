using Microsoft.EntityFrameworkCore;

namespace ChatalkApi.Models;

public class ChatalkDbContext : DbContext
{
    public ChatalkDbContext(DbContextOptions<ChatalkDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}